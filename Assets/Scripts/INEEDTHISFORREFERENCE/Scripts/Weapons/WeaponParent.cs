using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponParent : MonoBehaviour
{
    private static readonly int Reload = Animator.StringToHash("Reload");
    private enum SpawnWeaponStereotype
    {
        Undefined, 
        Spawn, 
        Respawn, 
        WeaponPickedUp, 
        PowerupCard, 
        SwitchAction
    }

    [SerializeField] private Image fillImage;
    [SerializeField] private Player player;
    [SerializeField] private GameObject initialWeapon;
    [SerializeField] private WeaponPickupData weaponPickupData;
    [SerializeField] private bool specialCaseKeepWeaponDisabled;
    [SerializeField] private WeaponSlots weaponSlots;
    private Weapon currentWeapon;
    private bool reloadSpikesActivated;
    private InputAction switchWeaponLeft;
    private InputAction switchWeaponRight;

    public Image FillImage => fillImage;
    public Weapon CurrentWeapon => currentWeapon;
    public bool IsMovementSpeedSlowedByShooting => player.PlayerMovement.IsMovementSlowedByShooting;
    public Player Player => player;
    public bool ReloadSpikesActivated { get => reloadSpikesActivated; set => reloadSpikesActivated = value; }

    private const int FIRST_WEAPON_SLOT_INDEX = 0;
    private const int SECOND_WEAPON_SLOT_INDEX = 1;

    public static event Action<Weapon> OnWeaponEquipped;

    private void OnEnable()
    {
        switchWeaponLeft = InputManager.Instance.PlayerInputActions.Player1.EquipPrimaryWeapon;
        switchWeaponRight = InputManager.Instance.PlayerInputActions.Player1.EquipSecondaryWeapon;
        switchWeaponLeft.Enable();
        switchWeaponRight.Enable();

        switchWeaponLeft.performed += OnSwitchWeaponLeft;
        switchWeaponRight.performed += OnSwitchWeaponRight;
    }

    private void OnDisable()
    {
        switchWeaponLeft.Disable();
        switchWeaponRight.Disable();

        switchWeaponLeft.performed -= OnSwitchWeaponLeft;
        switchWeaponRight.performed -= OnSwitchWeaponRight;
    }

    private void OnSwitchWeaponLeft(InputAction.CallbackContext context)
    {
        if (weaponSlots.currentlyEquippedSlot != SECOND_WEAPON_SLOT_INDEX)
            return;

        if (weaponSlots.slots[FIRST_WEAPON_SLOT_INDEX] is WeaponType.Undefined)
            return;

        weaponSlots.currentlyEquippedSlot = FIRST_WEAPON_SLOT_INDEX;
        Globals.PlayerOne.WeaponParent.SwitchWeaponCommand(weaponSlots.slots[weaponSlots.currentlyEquippedSlot]);
    }

    private void OnSwitchWeaponRight(InputAction.CallbackContext context)
    {
        if (weaponSlots.currentlyEquippedSlot != FIRST_WEAPON_SLOT_INDEX)
            return;

        if (weaponSlots.slots[SECOND_WEAPON_SLOT_INDEX] is WeaponType.Undefined)
            return;

        weaponSlots.currentlyEquippedSlot = SECOND_WEAPON_SLOT_INDEX;
        Globals.PlayerOne.WeaponParent.SwitchWeaponCommand(weaponSlots.slots[weaponSlots.currentlyEquippedSlot]);
    }
    
    private GameObject SpawnWeapon(GameObject weaponToSpawn, SpawnWeaponStereotype stereotype, out bool didEquipWeaponToEmptySlot, int extraAmmoOnPickup = 0)
    {
        GameObject weaponToSpawnOriginalPrefab = weaponPickupData.GetWeaponPrefab(weaponToSpawn);
        GameObject newlySpawnedHandHeldWeapon = Instantiate(weaponToSpawnOriginalPrefab, transform);
        SetNewWeaponTransform(newlySpawnedHandHeldWeapon, weaponToSpawnOriginalPrefab);
        Weapon newlySpawnedHandHeldWeaponScript = newlySpawnedHandHeldWeapon.GetComponent<Weapon>();
        newlySpawnedHandHeldWeaponScript.WeaponParent = this;
        
        // Initialize weapon ammo state based on stereotype
        switch (stereotype)
        {
            case SpawnWeaponStereotype.Spawn:
            case SpawnWeaponStereotype.PowerupCard:
                // Fresh weapon initialization
                newlySpawnedHandHeldWeaponScript.InitializeWeapon();
                break;

            case SpawnWeaponStereotype.Respawn:
            case SpawnWeaponStereotype.SwitchAction:
                // AmmoManager already has the magazine state for this WeaponType
                // Just need to ensure the weapon knows its type is initialized
                newlySpawnedHandHeldWeaponScript.RestoreWeaponState();
                break;

            case SpawnWeaponStereotype.WeaponPickedUp:
                // Pickup includes extra ammo
                newlySpawnedHandHeldWeaponScript.SetWeaponStateFromPickup(extraAmmoOnPickup);
                break;
            case SpawnWeaponStereotype.Undefined:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stereotype), stereotype, null);
        }
        
        if (newlySpawnedHandHeldWeaponScript.WeaponData.UsesDamageCollider)
            SetDamageColliderWeaponValues(newlySpawnedHandHeldWeaponScript);
        
        // Handle weapon slot management
        if (stereotype.Equals(SpawnWeaponStereotype.SwitchAction) || stereotype.Equals(SpawnWeaponStereotype.Respawn))
        {
            didEquipWeaponToEmptySlot = false;
        }
        else
        {
            didEquipWeaponToEmptySlot = LevelManager.Instance.WeaponSlots.TryEquipWeaponToEmptySlot(newlySpawnedHandHeldWeaponScript);

            if (!didEquipWeaponToEmptySlot)
                LevelManager.Instance.WeaponSlots.SetCurrentlyEquippedSlot(newlySpawnedHandHeldWeaponScript);
        }
        
        if (currentWeapon)
            Destroy(currentWeapon.gameObject);
        
        currentWeapon = newlySpawnedHandHeldWeaponScript;
        
        if (Globals.IsHomeBaseScene)
        {
            player.CanvasAmmo.SetActive(false);
            newlySpawnedHandHeldWeaponScript.WeaponLaser.gameObject.SetActive(false);
        }
        
        if (player)
            player.PlayerSpecialAbilityController.CurrentWeapon = newlySpawnedHandHeldWeaponScript;
        
        if (gameObject.activeInHierarchy)
            StartCoroutine(EnableWeaponDelayed());

        OnWeaponEquipped?.Invoke(newlySpawnedHandHeldWeaponScript);

        return newlySpawnedHandHeldWeapon;
    }

    private void SetNewWeaponTransform(GameObject newWeaponGameObject, GameObject newWeaponOriginalPrefab)
    {
        newWeaponGameObject.transform.localPosition = newWeaponOriginalPrefab.transform.position;
        newWeaponGameObject.transform.localEulerAngles = newWeaponOriginalPrefab.transform.eulerAngles;
        newWeaponGameObject.transform.localScale = newWeaponOriginalPrefab.transform.localScale;
    }

    private void SetDamageColliderWeaponValues(Weapon weapon)
    {
        DamageCollider damageCollider = weapon.DamageCollider.GetComponent<DamageCollider>();
        damageCollider.TimeBetweenDotTicks = weapon.WeaponData.TimeBetweenShots;
        damageCollider.IsWeapon = true;
        damageCollider.Weapon = weapon;
    }

    private IEnumerator EnableWeaponDelayed()
    {
        if (Globals.IsInMenu || Globals.IsHomeBaseScene || specialCaseKeepWeaponDisabled)
            yield break;
        else
            yield return null;

        currentWeapon.enabled = true;
    }

    private WeaponPickupInfo CreatePickupFromCurrentWeapon(Transform pickupParent)
    {
        currentWeapon.enabled = false;
        GameObject weaponGameObject = Instantiate(currentWeapon.gameObject, pickupParent);

        WeaponPickupInfo pickupInfo = new WeaponPickupInfo();

        if (weaponGameObject.TryGetComponent(out Weapon pickupWeapon))
        {
            pickupWeapon.HandleObjectsToSwitchEnabledStateOnPickup();
            pickupWeapon.WeaponLaser.gameObject.SetActive(false);
            
            // Store current weapon state for pickup
            pickupInfo.weaponGameObject = weaponGameObject;
            pickupInfo.magazineAmmo = currentWeapon.CurrentAmmo;
            
            // Calculate extra ammo for pickup (only if not infinite)
            if (!currentWeapon.WeaponData.NewAmmoStruct.HasInfiniteAmmo)
            {
                pickupInfo.extraAmmo = AmmoManager.Instance.GetReserveAmmo(currentWeapon.WeaponData.ammoType);
            }
        }

        return pickupInfo;
    }

    public GameObject OnWeaponPickup(GameObject pickedUpWeaponGO, Transform weaponPickupParent, out bool didEquipWeaponToEmptySlot)
    {
        // Create pickup from current weapon
        WeaponPickupInfo pickupInfo = CreatePickupFromCurrentWeapon(weaponPickupParent);
        
        // Get picked up weapon data
        Weapon pickedUpWeaponScript = pickedUpWeaponGO.GetComponent<Weapon>();

        // Spawn picked up weapon
        int weaponPickupExtraAmmo = pickedUpWeaponScript.WeaponData.NewAmmoStruct.HasInfiniteAmmo 
            ? 0 
            : pickedUpWeaponScript.WeaponData.NewAmmoStruct.DefaultExtraAmmo;
            
        SpawnWeapon(pickedUpWeaponGO, SpawnWeaponStereotype.WeaponPickedUp, out didEquipWeaponToEmptySlot, weaponPickupExtraAmmo);

        pickedUpWeaponScript.HandleObjectsToSwitchEnabledStateOnPickup();

        return pickupInfo.weaponGameObject;
    }

    public void ApplyMovementSpeedSlow(float movementSpeedSlowingFactor)
    {
        player.PlayerMovement.DecreaseSpeedByShooting(movementSpeedSlowingFactor);
    }

    public void RemoveMovementSpeedSlow(float movementSpeedSlowingFactor)
    {
        player.PlayerMovement.IncreaseSpeedAfterShooting(movementSpeedSlowingFactor);
    }

    public void SetWeaponOnSpawn(GameObject weaponGO, bool isRespawn)
    {
        Weapon weapon = weaponGO.GetComponent<Weapon>();

        SpawnWeaponStereotype stereotype = isRespawn ? SpawnWeaponStereotype.Respawn : SpawnWeaponStereotype.Spawn;

        SpawnWeapon(weaponGO, stereotype, out bool _);
    }

    public void SetWeaponFromPowerupCard(WeaponType weaponType)
    {
        SpawnWeapon(weaponPickupData.GetWeaponPrefab(weaponType), SpawnWeaponStereotype.PowerupCard, out bool _);
    }

    public void StartReloadAnimation()
    {
        player.CharacterAnimator.SetBool(Reload, true);
    }

    public void StopReloadAnimation()
    {
        player.CharacterAnimator.SetBool(Reload, false);
    }

    public void SwitchWeaponCommand(WeaponType weaponType)
    {
        SpawnWeapon(weaponPickupData.GetWeaponPrefab(weaponType), SpawnWeaponStereotype.SwitchAction, out bool _);
    }
}

/// <summary>
/// Helper struct for weapon pickup information
/// </summary>
public struct WeaponPickupInfo
{
    public GameObject weaponGameObject;
    public int magazineAmmo;
    public int extraAmmo;
}