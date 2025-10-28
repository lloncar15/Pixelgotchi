using UnityEngine;

public class LootObject : MonoBehaviour
{
    [SerializeField] public LootDropDefinition[] LootDropDefinitions;
    [SerializeField] private bool dropOnDestroy = false;
    [SerializeField] private bool lootAll = false;

    public void DropLoot()
    {
        if (lootAll)
        {
            LootManager.Instance.DropAllLootFromObject(LootDropDefinitions);
        }
        else
        {
            LootManager.Instance.DropLootFromObject(LootDropDefinitions);
        }
    }

    private void OnDestroy()
    {
        if (dropOnDestroy)
        {
            DropLoot();
        }
    }
}