using System;

/// <summary>
/// Data structure for all magazine information
/// </summary>
[Serializable]
public class MagazineData
{
	public WeaponType weaponType;
	public AmmoType ammoType;
	public int currentAmmo;
	public int maxAmmo;
	public bool hasInfiniteAmmo;
}