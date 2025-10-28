using System.Collections;
using UnityEngine;

public class ResourcePickup : PickupBase
{
    [Header("Resource")]
    [SerializeField] private CollectableType collectableType;
    [SerializeField] protected int amount;

    public override void CollectResource()
    {
        ResourceManager.Instance.CollectResource(collectableType, amount);
    }
}