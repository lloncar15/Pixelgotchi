using System;
using UnityEngine;

[Serializable]
public struct NewAmmoStruct
{
    public bool HasInfiniteAmmo;
    public int DefaultExtraAmmo;
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Main")][Space(10)]
    public string weaponName;
    public WeaponType weaponType;
    public Powerup weaponPowerup;
    public GameObject weaponPrefab;
    public Sprite weaponImageSprite;
    public float MaxReloadTime;
    public int MaxAmmo;
    public float TimeBetweenShots;
    public bool IsAutomatic;
    public AmmoType ammoType;
    [Space(10)] public NewAmmoStruct NewAmmoStruct;
    [Space(25)][Header("Movement Slow On Shoot")][Space(10)]
    public bool SlowMovementSpeedOnShoot;
    [Range(0, 1)] public float SlowingFactor;
    [Range(0, 10)] public float SlowDuration;

    [Space(25)][Header("Charging")][Space(10)]
    public bool IsChargeable;
    [Range(0f, 10f)] public float FirstChargingTime;
    [Range(0f, 10f)] public float SecondChargingTime;
    [Range(0f, 10f)] public float SpecialFirstChargingTime;
    [Range(0f, 10f)] public float SpecialSecondChargingTime;
    [Range(0f, 5f)] public float SpecialReloadTime;
    [Range(0f, 1f)] public float MinimumChargingTimeModifier;

    [Space(25)][Header("Damage Collider")][Space(10)]
    public bool UsesDamageCollider;
}