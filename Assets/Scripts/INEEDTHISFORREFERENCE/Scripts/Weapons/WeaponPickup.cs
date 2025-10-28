using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private float pickupCooldown = 3f;
    [SerializeField] private WeaponPickupData weaponPickupData;
    [SerializeField] private Transform pickupParent;
    [SerializeField] private GameObject effectActivePrefab;
    [SerializeField] private bool deactivateOnFirstPickup;
    private GameObject pickupableWeaponGO;
    private WeaponParent weaponParent;
    private bool canPickup = true;

    private void Start()
    {
        pickupableWeaponGO = Instantiate(weaponPickupData.weaponPickups[(int)weaponType].prefab, pickupParent);
        AdjustWeaponTransformForWeaponPickup(pickupableWeaponGO);
        
        Weapon initialPickupableWeapon = pickupableWeaponGO.GetComponent<Weapon>();
        initialPickupableWeapon.HandleObjectsToSwitchEnabledStateOnPickup();
        
        initialPickupableWeapon.InitializeWeapon();
    }
    
    public void TriggerEntered(Collider other)
    {
        if (!other.TryGetComponent(out Player player)) return;

        weaponParent = player.WeaponParent;
    }

    private void AdjustWeaponTransformForWeaponPickup(GameObject weapon)
    {
        WeaponType currentWeaponType = weapon.GetComponent<Weapon>().WeaponType;

        foreach (WeaponPickupTransformData currentData in weaponPickupData.weaponPickups.Where(currentData => currentWeaponType == currentData.prefab.GetComponent<Weapon>().WeaponType))
        {
            weapon.transform.localPosition = currentData.spawnLocalPosition;
            weapon.transform.localEulerAngles = currentData.spawnLocalEulerAngles;
            weapon.transform.localScale = currentData.spawnLocalScale;
            break;
        }
    }

    public void Pickup() {
        //TODO: provjeriti dal se mora provjeravati Globals.IsPlayerOneAlive
        if (!canPickup) return;
        
        GameObject newPickup = weaponParent.OnWeaponPickup(pickupableWeaponGO, pickupParent, out bool didEquipToEmptySlot);        
        Destroy(pickupableWeaponGO);
        
        if (didEquipToEmptySlot)
            gameObject.SetActive(false);
        else
        {
            pickupableWeaponGO = newPickup;
            AdjustWeaponTransformForWeaponPickup(pickupableWeaponGO);
            Instantiate(effectActivePrefab, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);
            //TODO: implementirati cooldown u InteractableObject pa maknuti ovdje
            Invoke(nameof(EnablePickup), pickupCooldown);
        }
        
        canPickup = false;

        if (deactivateOnFirstPickup)
            gameObject.SetActive(false);
    }

    private void EnablePickup()
    {
        canPickup = true;
    }
}