using System;
using System.Linq;
using UnityEngine;

public class Multibox : MonoBehaviour
{
    [SerializeField] private SpawnableResourceDefinition[] spawnableResourcesDefinitions;

    public void OnBoxDestroyed(Box destroyedBox)
    {
        SpawnStuff(destroyedBox);
    }

    private void SpawnStuff(Box destroyedBox)
    {
        float[] weights = spawnableResourcesDefinitions.Select(r => r.data.weight).ToArray();
        int pickedIndex = Utilities.PickWeightedIndex(weights);
        SpawnableResourceDefinition pickedResource = spawnableResourcesDefinitions[pickedIndex];

        if (pickedResource.spawnablePrefab is null)
        {
            Debug.Log("Spawning nothing");

            return;
        }

        int randomizedQuantity = UnityEngine.Random.Range(pickedResource.data.minQuantity, pickedResource.data.maxQuantity + 1);
        destroyedBox.SpawnDrops(pickedResource.spawnablePrefab, randomizedQuantity);

        Debug.Log("Spawning " + pickedResource.spawnablePrefab.name + ", quantity: " + randomizedQuantity);
    }

    // potencijalna generalizacija ove metode
    private Pair<int,float>[] GetIndexWeightPairs(SpawnableResourceDefinition[] spawnableResourcesDefinitions)
    {
        Pair<int, float>[] pairs = new Pair<int, float>[spawnableResourcesDefinitions.Length];

        for (int i = 0; i < pairs.Length; i++)
        {
            pairs[i] = new Pair<int, float>(i, spawnableResourcesDefinitions[i].data.weight);
        }

        return pairs;
    }
}