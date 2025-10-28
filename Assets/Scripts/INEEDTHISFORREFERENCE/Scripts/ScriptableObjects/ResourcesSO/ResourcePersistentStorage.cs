using UnityEngine;

public static class ResourcePersistentStorage
{
    public static void SaveAll(ResourceBase[] resources)
    {
        foreach (ResourceBase resource in resources)
        {
            Save(resource);
        }
    }
    
    public static void Save(ResourceBase resource)
    {
        string json = JsonUtility.ToJson(resource.data);
        PlayerPrefs.SetString(resource.GetName(), json);
        PlayerPrefs.Save();
    }
    
    public static void LoadAll(ResourceBase[] resources)
    {
        foreach (ResourceBase resource in resources)
        {
            Load(resource);
        }
    }

    public static void Load(ResourceBase resource) {
        string key = resource.GetName();
        if (PlayerPrefs.HasKey(key))
        {
            string json = PlayerPrefs.GetString(key);
            JsonUtility.FromJsonOverwrite(json, resource.data);
        }
    }
    
    public static void ClearAll(ResourceBase[] resources)
    {
        foreach (ResourceBase resource in resources)
        {
            Clear(resource);
        }

        PlayerPrefs.Save();
    }
    
    public static void Clear(ResourceBase resource, bool shouldSave = false)
    {
        string key = resource.GetName();
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
        }
        
        if (shouldSave)
        {
            PlayerPrefs.Save();
        }
    }
}