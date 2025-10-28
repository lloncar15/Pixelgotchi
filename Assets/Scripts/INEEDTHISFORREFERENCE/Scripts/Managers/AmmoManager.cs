using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
	[SerializeField] private Ammo[] ammoResources;
	
	/// <summary>
	/// Core ammo data structures.
	/// AmmoLookup used to check the overall ammo data.
	/// MagazineData used for managing the current magazine data of equipped weapons.
	/// </summary>
	private readonly Dictionary<AmmoType, Ammo> ammoLookup = new();
	private readonly Dictionary<WeaponType, MagazineData> magazineData = new();
	
	private static AmmoManager instance;
	public static AmmoManager Instance
	{
		get
		{
			if (!instance)
			{
				instance = FindAnyObjectByType<AmmoManager>();
				if (!instance)
				{
					GameObject gameObject = new GameObject($"{nameof(AmmoManager)} (Auto-Generated)");
					instance = gameObject.AddComponent<AmmoManager>();
				}
			}

			return instance;
		}
	}

	public static event Action<AmmoType, int> OnReserveAmmoChanged;
	/// <summary>
	/// Called when magazine ammo is changed with the following parameters:
	/// WeaponType, CurrentAmmo, MaxAmmo
	/// </summary>
	public static event Action<WeaponType, int, int> OnMagazineAmmoChanged;
	public static event Action<WeaponType> OnReloadStarted;
	public static event Action<WeaponType> OnReloadCompleted;

	private void Awake()
	{
		if (instance)
		{
			instance = this;
		}

		InitializeAmmoLookup();
	}

	private void Start()
	{
		LoadAllAmmo();
	}

	private void OnApplicationQuit()
	{
		SaveAllAmmo();
	}
	
	private void InitializeAmmoLookup()
	{
		ammoLookup.Clear();
		foreach (Ammo ammo in ammoResources)
		{
			if (ammo)
			{
				ammoLookup[ammo.AmmoType] = ammo;
			}
		}
	}
#region Reserve Ammo Management

	/// <summary>
	/// Add reserve ammo to the ammo ResourceBase (collected from pickups, etc.)
	/// </summary>
	public void AddReserveAmmo(AmmoType ammoType, int amount)
	{
		if (ammoLookup.TryGetValue(ammoType, out Ammo ammo))
		{
			ammo.AddAmount(amount);
			OnReserveAmmoChanged?.Invoke(ammoType, ammo.CurrentAmount);
		}
		else
		{
			Debug.LogError($"AmmoType {ammoType} not found in AmmoManager!");
		}
	}
	
	/// <summary>
	/// Try to spend reserve ammo during reloading.
	/// </summary>
	public bool TrySpendReserveAmmo(AmmoType ammoType, int amount)
	{
		if (!ammoLookup.TryGetValue(ammoType, out Ammo ammo))
			return false;

		if (!ammo.TrySpendAmount(amount))
			return false;
		
		OnReserveAmmoChanged?.Invoke(ammoType, ammo.CurrentAmount);
		return true;
	}
	
	/// <summary>
	/// Force spend reserve ammo even if there isn't enough
	/// </summary>
	public void ForceSpendReserveAmmo(AmmoType ammoType, int amount)
	{
		if (ammoLookup.TryGetValue(ammoType, out Ammo ammo))
		{
			int currentAmount = ammo.CurrentAmount;
			int newAmount = Mathf.Max(0, currentAmount - amount);
			
			ammo.data.currentAmount = newAmount;
			OnReserveAmmoChanged?.Invoke(ammoType, newAmount);
		}
		else
		{
			Debug.LogError($"AmmoType {ammoType} not found in AmmoManager!");
		}
	}
	
	/// <summary>
	/// Sets the amount in the ammo reserve.
	/// </summary>
	public void SetReserveAmmo(AmmoType ammoType, int amount)
	{
		if (ammoLookup.TryGetValue(ammoType, out Ammo ammo))
		{
			ammo.ResetData();
			ammo.AddAmount(amount);
			OnReserveAmmoChanged?.Invoke(ammoType, ammo.CurrentAmount);
		}
		else
		{
			Debug.LogError($"AmmoType {ammoType} not found in AmmoManager!");
		}
	}
	
	/// <summary>
	/// Gets the amount of ammo in the reserve.
	/// </summary>
	/// <param name="ammoType">AmmoType to get.</param>
	/// <returns>Amount of ammo in reserve for the AmmoType.</returns>
	public int GetReserveAmmo(AmmoType ammoType)
	{
		return ammoLookup.TryGetValue(ammoType, out Ammo ammo) ? ammo.CurrentAmount : 0;
	}

	/// <summary>
	/// Checks if there is enough ammo in the reserve for a specified AmmoType.
	/// </summary>
	/// <param name="ammoType">AmmoType to check.</param>
	/// <param name="amount">Amount of ammo needed from the reserve.</param>
	/// <returns>True if enough ammo in the reserve.</returns>
	public bool HasReserveAmmo(AmmoType ammoType, int amount = 1)
	{
		return GetReserveAmmo(ammoType) >= amount;
	}

	/// <summary>
	/// Gets the Ammo ScriptableObject for an AmmoType.
	/// </summary>
	/// <param name="ammoType">AmmoType to get.</param>
	/// <returns>Ammo as ResourceBase or null if it doesn't exist in the AmmoManager.</returns>
	public Ammo GetAmmo(AmmoType ammoType)
	{
		return ammoLookup.GetValueOrDefault(ammoType);
	}
	
	/// <summary>
	/// Gets the amounts dictionary for all AmmoTypes in the Manager.
	/// </summary>
	public Dictionary<AmmoType, int> GetAllReserveAmmoAmounts()
	{
		Dictionary<AmmoType, int> amounts = new();
		foreach (KeyValuePair<AmmoType, Ammo> kvp in ammoLookup)
		{
			amounts[kvp.Key] = kvp.Value.CurrentAmount;
		}
		return amounts;
	}

#endregion

#region Magazine Management

	/// <summary>
	/// Initialize magazine for a weapon type.
	/// </summary>
	public void InitializeMagazine(WeaponType weaponType, AmmoType ammoType, int maxAmmo, int currentAmmo, bool hasInfiniteAmmo)
	{
		if (!magazineData.ContainsKey(weaponType))
		{
			magazineData[weaponType] = new MagazineData();
		}

		MagazineData data = magazineData[weaponType];
		data.weaponType = weaponType;
		data.ammoType = ammoType;
		data.maxAmmo = maxAmmo;
		data.currentAmmo = currentAmmo;
		data.hasInfiniteAmmo = hasInfiniteAmmo;
		
		OnMagazineAmmoChanged?.Invoke(weaponType, data.currentAmmo, data.maxAmmo);
	}
	
	/// <summary>
	/// Set magazine ammo for a specific weapon (used when switching weapons)
	/// </summary>
	public void SetMagazineAmmo(WeaponType weaponType, int currentAmmo)
	{
		if (magazineData.TryGetValue(weaponType, out MagazineData data))
		{
			data.currentAmmo = Mathf.Clamp(currentAmmo, 0, data.maxAmmo);
			OnMagazineAmmoChanged?.Invoke(weaponType, data.currentAmmo, data.maxAmmo);
		}
		else
		{
			Debug.LogError($"Magazine not initialized for weapon type {weaponType}");
		}
	}
	
	/// <summary>
	/// Try to spend magazine ammo for shooting.
	/// Weapons with infinite ammo don't spend ammo.
	/// </summary>
	/// <param name="weaponType">WeaponType to spend magazine ammo from.</param>
	/// <param name="amount">Amount to spend.</param>
	/// <returns>True if spending is successful or weapon has infinite ammo.</returns>
	public bool TrySpendMagazineAmmo(WeaponType weaponType, int amount = 1)
	{
		if (!magazineData.TryGetValue(weaponType, out MagazineData data))
		{
			Debug.LogError($"Magazine not initialized for weapon type {weaponType}");
			return false;
		}
		
		//TODO: check for this?
		if (data.hasInfiniteAmmo)
		{
			return true;
		}

		if (data.currentAmmo < amount)
			return false;

		data.currentAmmo -= amount;
		OnMagazineAmmoChanged?.Invoke(weaponType, data.currentAmmo, data.maxAmmo);
		
		return true;
	}
	
	/// <summary>
	/// Force spend magazine ammo for special mechanics.
	/// </summary>
	public void ForceSpendMagazineAmmo(WeaponType weaponType, int amount)
	{
		if (!magazineData.TryGetValue(weaponType, out MagazineData data))
			return;
		
		if (data.hasInfiniteAmmo)
			return;

		data.currentAmmo = Mathf.Max(0, data.currentAmmo - amount);
		OnMagazineAmmoChanged?.Invoke(weaponType, data.currentAmmo, data.maxAmmo);
	}
	
	/// <summary>
	/// Get current magazine ammo for weapon.
	/// </summary>
	/// <param name="weaponType">WeaponType to get ammo for.</param>
	/// <returns>Current ammo in the magazine</returns>
	public int GetMagazineAmmo(WeaponType weaponType)
	{
		return magazineData.TryGetValue(weaponType, out MagazineData data) ? data.currentAmmo : 0;
	}
	
	/// <summary>
	/// Get max magazine ammo for a weapon
	/// </summary>
	/// <param name="weaponType">WeaponType to get ammo for.</param>
	/// <returns>Max ammo in the magazine</returns>
	public int GetMaxMagazineAmmo(WeaponType weaponType)
	{
		return magazineData.TryGetValue(weaponType, out MagazineData data) ? data.maxAmmo : 0;
	}
	
	/// <summary>
	/// Check if magazine is empty.
	/// </summary>
	/// <returns>True if there is no ammo in the magazine.</returns>
	public bool IsMagazineEmpty(WeaponType weaponType)
	{
		return GetMagazineAmmo(weaponType) <= 0;
	}
	
	/// <summary>
	/// Check if magazine is full
	/// </summary>
	public bool IsMagazineFull(WeaponType weaponType)
	{
		if (!magazineData.TryGetValue(weaponType, out MagazineData data))
			return false;

		return data.currentAmmo >= data.maxAmmo;
	}

	/// <summary>
	/// Increase max magazine capacity (use-case: upgrades)
	/// </summary>
	/// <param name="weaponType">WeaponType to get the magazine for.</param>
	/// <param name="amount">Amount to increase.</param>
	public void IncreaseMaxMagazineCapacity(WeaponType weaponType, int amount)
	{
		if (!magazineData.TryGetValue(weaponType, out MagazineData data))
			return;
		
		data.maxAmmo += amount;
		OnMagazineAmmoChanged?.Invoke(weaponType, data.currentAmmo, data.maxAmmo);
	}
	
	/// <summary>
	/// Calculates how much ammo can be reloaded
	/// </summary>
	/// <param name="weaponType">Weapon type to get reload info for.</param>
	public ReloadInfo CalculateReload(WeaponType weaponType)
	{
		if (!magazineData.TryGetValue(weaponType, out MagazineData data))
		{
			return new ReloadInfo { isValid = false };
		}

		ReloadInfo info = new ReloadInfo
		{
			isValid = true,
			weaponType = weaponType,
			hasInfiniteAmmo = data.hasInfiniteAmmo,
			missingAmmoInMagazine = data.maxAmmo - data.currentAmmo
		};

		if (data.hasInfiniteAmmo)
		{
			info.ammoToAdd = info.missingAmmoInMagazine;
			info.availableReserveAmmo = int.MaxValue;
		}
		else
		{
			info.availableReserveAmmo = GetReserveAmmo(data.ammoType);
			info.ammoToAdd = Mathf.Min(info.missingAmmoInMagazine, info.availableReserveAmmo);
		}

		return info;
	}
	
	/// <summary>
	/// Performs reloading by transferring ammo from reserve to magazine.
	/// </summary>
	/// <param name="weaponType">WeaponType to reload for.</param>
	/// <returns>True if reload performed successfully.</returns>
	public bool PerformReload(WeaponType weaponType)
	{
		if (!magazineData.TryGetValue(weaponType, out MagazineData data))
		{
			Debug.LogError($"Magazine not initialized for weapon type {weaponType}");
			return false;
		}

		ReloadInfo reloadInfo = CalculateReload(weaponType);
		
		if (!reloadInfo.isValid || reloadInfo.ammoToAdd <= 0)
			return false;

		OnReloadStarted?.Invoke(weaponType);

		// Spend reserve ammo if not infinite
		if (!data.hasInfiniteAmmo)
		{
			ForceSpendReserveAmmo(data.ammoType, reloadInfo.ammoToAdd);
		}

		// Add to magazine
		data.currentAmmo += reloadInfo.ammoToAdd;
		OnMagazineAmmoChanged?.Invoke(weaponType, data.currentAmmo, data.maxAmmo);
		OnReloadCompleted?.Invoke(weaponType);

		return true;
	}
	
	/// <summary>
	/// Complete instant reload for special abilities.
	/// </summary>
	/// <param name="weaponType"></param>
	public void InstantReload(WeaponType weaponType)
	{
		if (!magazineData.TryGetValue(weaponType, out MagazineData data))
			return;
		
		ReloadInfo reloadInfo = CalculateReload(weaponType);
			
		if (!data.hasInfiniteAmmo && reloadInfo.ammoToAdd > 0)
		{
			ForceSpendReserveAmmo(data.ammoType, reloadInfo.ammoToAdd);
		}

		data.currentAmmo = data.maxAmmo;
		OnMagazineAmmoChanged?.Invoke(weaponType, data.currentAmmo, data.maxAmmo);
	}

	/// <summary>
	/// Get read-only magazine data for a weapon.
	/// </summary>
	/// <param name="weaponType">WeaponType to get magazine data for.</param>
	/// <returns>MagazineData for the weapon or null if it doesn't exist in the manager.</returns>
	[CanBeNull] public MagazineData GetMagazineData(WeaponType weaponType)
	{
		return magazineData.GetValueOrDefault(weaponType);
	}

  #endregion

#region Serialization

	//TODO: Check magazine data serialization
	private void SaveAllAmmo()
	{
		foreach (Ammo ammo in ammoResources)
		{
			if (ammo)
			{
				ResourcePersistentStorage.Save(ammo);
			}
		}
	}

	private void LoadAllAmmo()
	{
		foreach (Ammo ammo in ammoResources)
		{
			if (ammo)
			{
				ResourcePersistentStorage.Load(ammo);
			}
		}
	}

	public void ClearAllAmmo()
	{
		foreach (Ammo ammo in ammoResources)
		{
			if (ammo)
			{
				ResourcePersistentStorage.Clear(ammo);
				ammo.ResetData();
			}
		}
		
		magazineData.Clear();
	}

#endregion

#if UNITY_EDITOR
	/// <summary>
	/// Editor-only method to manually add ammo for testing
	/// </summary>
	public void EditorAddAmmo(Ammo ammo, int amount)
	{
		if (!ammo) 
			return;
        
		AddReserveAmmo(ammo.AmmoType, amount);
	}
    
	/// <summary>
	/// Editor-only method to reset ammo
	/// </summary>
	public void EditorResetAmmo(Ammo ammo)
	{
		if (!ammo) 
			return;
        
		ammo.ResetData();
		OnReserveAmmoChanged?.Invoke(ammo.AmmoType, 0);
	}
#endif
}

/// <summary>
/// Information sent when reloading a weapon.
/// </summary>
public struct ReloadInfo
{
	public bool isValid;
	public WeaponType weaponType;
	public bool hasInfiniteAmmo;
	public int missingAmmoInMagazine;
	public int availableReserveAmmo;
	public int ammoToAdd;
}