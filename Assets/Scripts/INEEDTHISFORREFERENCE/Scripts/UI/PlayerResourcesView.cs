using TMPro;
using UnityEngine;
using NUnit.Framework;

public class PlayerResourcesView : MonoBehaviour
{
    [Header("Resource References")]
    [SerializeField] private ResourceBase coinsResource;
    [SerializeField] private ResourceBase scrapsResource;
    
    [SerializeField] private TMP_Text cashAmountText;
    [SerializeField] private TMP_Text scrapsAmountText;

#if UNITY_EDITOR
    private void OnValidate()
    {
        Assert.IsNotNull(cashAmountText, "PlayerResourcesView: cashAmountText is null!");
        Assert.IsNotNull(scrapsAmountText, "PlayerResourcesView: scrapsAmountText is null!");
    }
#endif

    private void OnEnable()
    {
        ResourceManager.OnResourceCollected += OnResourceChanged;
        ResourceManager.OnResourceSpent += OnResourceChanged;
    }

    private void OnDisable()
    {
        ResourceManager.OnResourceCollected -= OnResourceChanged;
        ResourceManager.OnResourceSpent -= OnResourceChanged;
    }

    public void Start()
    {
        UpdateCashUI();
        UpdateScrapsUI();
    }

    private void OnResourceChanged(ResourceBase resource, int amount)
    {
        if (resource == coinsResource)
        {
            UpdateCashUI();
        }
        else if (resource == scrapsResource)
        {
            UpdateScrapsUI();
        }
    }

    private void UpdateCashUI()
    {
        if (coinsResource is null)
            return;
        
        cashAmountText.text = coinsResource.CurrentAmount.ToString();
    }

    private void UpdateScrapsUI()
    {
        if (scrapsResource is null)
            return;
        
        scrapsAmountText.text = scrapsResource.CurrentAmount.ToString();
    }
}