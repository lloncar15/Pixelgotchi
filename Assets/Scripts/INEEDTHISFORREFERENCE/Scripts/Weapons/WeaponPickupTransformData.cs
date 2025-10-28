using UnityEngine;

[System.Serializable]
public struct WeaponPickupTransformData
{
    public GameObject prefab;
    public Vector3 spawnLocalPosition;
    public Vector3 spawnLocalEulerAngles;
    public Vector3 spawnLocalScale;
}