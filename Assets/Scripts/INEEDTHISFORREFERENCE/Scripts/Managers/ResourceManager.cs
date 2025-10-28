using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private ResourceConfiguration resourceConfiguration;
    public ResourceConfiguration ResourceConfiguration => resourceConfiguration;

    private static ResourceManager instance;
    public static ResourceManager Instance => instance;

    public static event Action<ResourceBase, int> OnResourceCollected;
    public static event Action<ResourceBase, int> OnResourceSpent;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            return;
        }

        Debug.LogError("Duplicate resources manager.", instance);
        Debug.LogError("Duplicate resources manager.", this);
    }

    private void Start()
    {
        //TODO: koristiti dok se ubaci Save/Load
        // LoadAllResources();
    }
    
    private void OnApplicationQuit()
    {
        //TODO: koristiti dok se ubaci Save/Load
        // SaveAllResources();
    }

    public void CollectResource(ResourceBase resource, int amount)
    {
        if (resource is null)
        {
            Debug.LogError("Cannot collect null resource.");
            return;
        }
        
        resource.AddAmount(amount);
        
        OnResourceCollected?.Invoke(resource, amount);
        ObjectivesManager.Instance?.CheckCollectableObjectiveCompletion(resource.CollectableType, amount);
    }
    
    public void CollectResource(CollectableType collectableType, int amount)
    {
        ResourceBase resource = resourceConfiguration.GetResourceByType(collectableType);
        
        if (resource == null)
        {
            Debug.LogError($"Cannot collect resource: No resource found for {collectableType}");
            return;
        }

        CollectResource(resource, amount);
    }

    public bool TrySpendResource(ResourceBase resource, int amount)
    {
        if (resource is null)
        {
            Debug.LogError("Cannot spend null resource.");
            return false;
        }

        if (resource.TrySpendAmount(amount))
        {
            OnResourceSpent?.Invoke(resource, amount);
            return true;
        }
        
        return false;
    }

    public bool TrySpendResource(CollectableType collectableType, int amount)
    {
        ResourceBase resource = resourceConfiguration.GetResourceByType(collectableType);

        if (resource is null)
        {
            Debug.LogError($"Cannot spend resource: No resource found for {collectableType}");
            return false;
        }

        return TrySpendResource(resource, amount);
    }

#region Getters

    public ResourceBase GetResource(CollectableType type)
    {
        return resourceConfiguration.GetResourceByType(type);
    }

    public int GetResourceAmount(ResourceBase resource)
    {
        return resource?.CurrentAmount ?? 0;
    }

    public int GetResourceAmount(CollectableType type)
    {
        ResourceBase resource = resourceConfiguration.GetResourceByType(type);
        return resource?.CurrentAmount ?? 0;
    }

    public int GetResourceCollectedThisScene(ResourceBase resource)
    {
        return resource?.CollectedThisScene ?? 0;
    }

    public int GetResourceCollectedThisScene(CollectableType type)
    {
        ResourceBase resource = resourceConfiguration.GetResourceByType(type);
        return resource?.CollectedThisScene ?? 0;
    }

#endregion
#region Resource Persistence

    public void ResetSceneCollections()
    {
        ResourceBase[] allResources = resourceConfiguration.GetAllResources();
        foreach (ResourceBase resource in allResources)
        {
            resource.ResetSceneCollection();
        }
    }
    
    private void SaveAllResources()
    {
        ResourceBase[] allResources = resourceConfiguration.GetAllResources();
        ResourcePersistentStorage.SaveAll(allResources);
    }

    private void LoadAllResources()
    {
        ResourceBase[] allResources = resourceConfiguration.GetAllResources();
        ResourcePersistentStorage.LoadAll(allResources);
    }

    public void ClearAllResources()
    {
        ResourceBase[] allResources = resourceConfiguration.GetAllResources();
        ResourcePersistentStorage.ClearAll(allResources);
        
        foreach (ResourceBase resource in allResources)
        {
            resource.ResetData();
        }
    }
    
#endregion

#if UNITY_EDITOR
    /// <summary>
    /// Editor-only method to manually trigger resource collection for testing
    /// </summary>
    public void EditorAddResource(ResourceBase resource, int amount)
    {
        if (resource is null) 
            return;
        
        resource.AddAmount(amount);
        OnResourceCollected?.Invoke(resource, amount);
    }
    
    /// <summary>
    /// Editor-only method to reset a resource
    /// </summary>
    public void EditorResetResource(ResourceBase resource)
    {
        if (resource is null) 
            return;
        
        resource.ResetData();
        OnResourceCollected?.Invoke(resource, 0);
    }
#endif
}