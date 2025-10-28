using System.Runtime.Remoting.Messaging;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class PlayerStatusView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text weaponText;
    [SerializeField] private TMP_Text ammoText;

    private Weapon currentWeapon;

#if UNITY_EDITOR
    private void OnValidate()
    {
        Assert.IsNotNull(healthText, "PlayerStatusView: HealthText is null!");
        Assert.IsNotNull(weaponText, "PlayerStatusView: WeaponText is null!");
        Assert.IsNotNull(ammoText, "PlayerStatusView: AmmoText is null!");
    }
#endif

    private void OnEnable()
    {
        PlayerHealth.OnHealthChecked += UpdateHealth;
        AmmoManager.OnMagazineAmmoChanged -= UpdateMagazineAmmo;
        AmmoManager.OnReserveAmmoChanged += UpdateReserveAmmo;
        WeaponParent.OnWeaponEquipped += UpdateWeapon;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChecked -= UpdateHealth;
        AmmoManager.OnMagazineAmmoChanged -= UpdateMagazineAmmo;
        AmmoManager.OnReserveAmmoChanged += UpdateReserveAmmo;
        WeaponParent.OnWeaponEquipped -= UpdateWeapon;
    }

    public void Start()
    {
        // Special case for first Player/Weapon Spawn (happens earlier than event subscription)
        if (Globals.PlayerOne && Globals.PlayerOne.Weapon)
        {
            UpdateWeapon(Globals.PlayerOne.Weapon);
        }
    }

    private void UpdateHealth()
    {
        healthText.text = $"{Mathf.CeilToInt(Globals.PlayerOne.PlayerHealth.CurrentPlayerHitpoints)} HP";
    }

    private void UpdateWeapon(Weapon newlyEquippedWeapon)
    {
        currentWeapon = newlyEquippedWeapon;
        weaponText.text = newlyEquippedWeapon.WeaponData.weaponName;
        UpdateAmmoDisplay();
    }
    
    private void UpdateMagazineAmmo(WeaponType weaponType, int currentAmmo, int maxAmmo)
    {
        if (currentWeapon && currentWeapon.WeaponType == weaponType)
        {
            UpdateAmmoDisplay();
        }
    }

    private void UpdateReserveAmmo(AmmoType ammoType, int reserveAmount)
    {
        if (currentWeapon && currentWeapon.WeaponData.ammoType == ammoType)
        {
            UpdateAmmoDisplay();
        }
    }

    private void UpdateAmmoDisplay()
    {
        if (currentWeapon)
            return;

        if (currentWeapon.WeaponData.NewAmmoStruct.HasInfiniteAmmo)
        {
            ammoText.text = $"{currentWeapon.CurrentAmmo}/Inf";
        }
        else
        {
            int reserveAmmo = AmmoManager.Instance.GetReserveAmmo(currentWeapon.WeaponData.ammoType);
            ammoText.text = $"{currentWeapon.CurrentAmmo}/{reserveAmmo}";
        }
    }
}