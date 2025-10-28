using System;
using UnityEngine;

[Serializable]
public class ResourceData
{
    public int currentAmount;
    public int totalCollectedAmount;
    public int collectedThisScene;
    public CollectableType collectableType;
}

public abstract class ResourceBase : ScriptableObject {
    [SerializeField] public ResourceData data = new();

    public int CurrentAmount => data.currentAmount;
    public int TotalCollectedAmount => data.totalCollectedAmount;
    public int CollectedThisScene => data.collectedThisScene;
    public CollectableType CollectableType => data.collectableType;

    public virtual string GetName() => nameof(Scraps);

    public virtual void AddAmount(int amount)
    {
        data.currentAmount += amount;
        data.totalCollectedAmount += amount;
        data.collectedThisScene += amount;
    }

    public virtual bool TrySpendAmount(int amount)
    {
        if (data.currentAmount < amount)
            return false;

        data.currentAmount -= amount;
        return true;
    }

    public virtual void ResetSceneCollection()
    {
        data.collectedThisScene = 0;
    }

    public virtual void ResetData()
    { 
        data.currentAmount = 0;
        data.totalCollectedAmount = 0;
        data.collectedThisScene = 0;
    }
}