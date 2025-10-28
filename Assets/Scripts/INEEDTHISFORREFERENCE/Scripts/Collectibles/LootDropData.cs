using System;
using UnityEngine;

[Serializable]
public class LootDropData
{
    public float weight = 1f;
    public int minQuantity = 1;
    public int maxQuantity = 1;
}

[Serializable]
public class LootDropDefinition
{
    public ResourceBase resource;
    public LootDropData dropData;
}

[Serializable]
public class SpawnableResourceDefinition
{
    public GameObject spawnablePrefab;
    public LootDropData data;
}