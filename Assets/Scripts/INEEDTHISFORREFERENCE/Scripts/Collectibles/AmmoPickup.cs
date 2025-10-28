using System.Collections;
using UnityEngine;

public class AmmoPickup : PickupBase
{
    [Header("Ammo")]
    [SerializeField] private Ammo ammoResource;
    [SerializeField] protected int ammoAmount = 10;

    public override void CollectResource()
    {
        if (!ammoResource)
        {
            Debug.LogError($"AmmoPickup on {gameObject.name} has no ammo resource assigned!", this);
            return;
        }
        
        AmmoManager.Instance.AddReserveAmmo(ammoResource.AmmoType, ammoAmount);
    }
}