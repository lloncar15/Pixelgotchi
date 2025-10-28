using System.Linq;
using UnityEngine;
public class LootManager : MonoBehaviour
{
	private static LootManager instance;
	public static LootManager Instance
	{
		get
		{
			if (!instance)
			{
				instance = FindAnyObjectByType<LootManager>();
				if (!instance)
				{
					var gameObject = new GameObject($"{nameof(LootManager)} (Auto-Generated)");
					instance = gameObject.AddComponent<LootManager>();
				}
			}

			return instance;
		}
	}
	
	private void Awake()
	{
		if (!instance)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void DropLootFromObject(LootDropDefinition[] lootDropDefinitions)
	{
		if (lootDropDefinitions is null || lootDropDefinitions.Length == 0)
		{
			Debug.LogWarning($"LootObject has no loot definitions.");
			return;
		}
		
		float[] weights = lootDropDefinitions.Select(d => d.dropData.weight).ToArray();
		int pickedIndex = Utilities.PickWeightedIndex(weights);
		LootDropDefinition pickedDrop = lootDropDefinitions[pickedIndex];

		if (pickedDrop.resource is null)
		{
			Debug.LogWarning($"LootObject has a null resource in drop definition at index {pickedIndex}");
			return;
		}
		
		int randomizedQuantity = Random.Range(pickedDrop.dropData.minQuantity, pickedDrop.dropData.maxQuantity + 1);
		
		CollectResource(pickedDrop.resource, randomizedQuantity);
	}

	public void DropAllLootFromObject(LootDropDefinition[] lootDropDefinitions)
	{
		if (lootDropDefinitions is null || lootDropDefinitions.Length == 0)
		{
			Debug.LogWarning($"LootObject has no loot definitions.");
			return;
		}

		foreach (LootDropDefinition loot in lootDropDefinitions)
		{
			int randomizedQuantity = Random.Range(loot.dropData.minQuantity, loot.dropData.maxQuantity + 1);
			CollectResource(loot.resource, randomizedQuantity);
		}
	}
	
	private void CollectResource(ResourceBase resource, int amount)
	{
		Debug.Log($"Collecting resource {resource.name}:  {amount}");
		if (resource is Ammo ammo)
		{
			AmmoManager.Instance.AddReserveAmmo(ammo.AmmoType, amount);
		}
		else
		{
			ResourceManager.Instance.CollectResource(resource, amount);
		}
	}
}