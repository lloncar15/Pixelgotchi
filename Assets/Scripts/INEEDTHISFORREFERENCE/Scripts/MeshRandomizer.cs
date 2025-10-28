using UnityEngine;

public class MeshRandomizer : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Mesh[] meshes;

    private void Awake()
    {
        meshFilter.mesh = meshes[Random.Range(0, meshes.Length)];
    }
}