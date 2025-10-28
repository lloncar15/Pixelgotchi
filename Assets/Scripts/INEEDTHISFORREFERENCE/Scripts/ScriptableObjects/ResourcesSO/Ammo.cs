using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo", menuName = "Resources/Ammo")]
public class Ammo : ResourceBase
{
    [SerializeField] private AmmoType ammoType;
    public AmmoType AmmoType => ammoType;

    public override string GetName() => AmmoType.ToString();
}