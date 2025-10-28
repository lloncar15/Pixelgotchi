using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ResourceConfiguration", menuName = "Resources/Configuration")]
public class ResourceConfiguration : ScriptableObject
{
	[Tooltip("Do NOT include Ammo resources here - they're managed by AmmoManager.")]
	[SerializeField] private ResourceBase[] resources;
	
	public ResourceBase[] GetAllResources() => resources;

	public ResourceBase GetResourceByType(CollectableType type)
	{
		foreach (ResourceBase resource in resources)
		{
			if (resource.CollectableType == type && resource is not Ammo)
			{
				return resource;
			}
		}
		
		Debug.LogWarning($"No resource found for CollectableType: {type}");
		return null;
	}
}