using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSlots", menuName = "Weapon/Weapon Slots")]
public class WeaponSlots : ScriptableObject
{
    public WeaponType[] slots;
    public int currentlyEquippedSlot = 0;

    public bool TryEquipWeaponToEmptySlot(Weapon weapon)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] is WeaponType.Undefined)
            {
                slots[i] = weapon.WeaponType;
                currentlyEquippedSlot = i;
                return true;
            }
        }

        Debug.Log("Could not add weapon to empty slot, did not find viable empty slot for weapon addition");

        return false;
    }

    public void SetCurrentlyEquippedSlot(Weapon weapon)
    {
        slots[currentlyEquippedSlot] = weapon.WeaponType;
    }

    public void AddInitialSecondaryWeapon(WeaponData initialSecondaryWeapon)
    {
        slots[1] = initialSecondaryWeapon.weaponType;
        AmmoManager.Instance.InitializeMagazine(
            initialSecondaryWeapon.weaponType,
            initialSecondaryWeapon.ammoType,
            initialSecondaryWeapon.MaxAmmo,
            initialSecondaryWeapon.MaxAmmo,
            initialSecondaryWeapon.NewAmmoStruct.HasInfiniteAmmo);
        
        if (!initialSecondaryWeapon.NewAmmoStruct.HasInfiniteAmmo)
        {
            AmmoManager.Instance.SetReserveAmmo(initialSecondaryWeapon.ammoType, initialSecondaryWeapon.NewAmmoStruct.DefaultExtraAmmo);
        }
    }

    public void ResetSlotsData()
    {
        currentlyEquippedSlot = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = WeaponType.Undefined;
        }
    }
}