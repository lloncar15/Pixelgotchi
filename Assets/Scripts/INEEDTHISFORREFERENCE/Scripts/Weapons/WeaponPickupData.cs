using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponPickupData", menuName = "Weapon/Weapon Pickup Data")]
public class WeaponPickupData : ScriptableObject
{
    public List<WeaponPickupTransformData> weaponPickups;

    public GameObject GetWeaponPrefab(GameObject weapon)
    {
        WeaponType currentWeaponType = weapon.GetComponent<Weapon>().WeaponType;

        foreach (var v in weaponPickups)
        {
            if (currentWeaponType == v.prefab.GetComponent<Weapon>().WeaponType)
                return v.prefab;
        }

        return null;
    }

    public GameObject GetWeaponPrefab(WeaponType weaponType)
    {
        foreach (var v in weaponPickups)
        {
            if (weaponType == v.prefab.GetComponent<Weapon>().WeaponType)
                return v.prefab;
        }

        return null;
    }
}