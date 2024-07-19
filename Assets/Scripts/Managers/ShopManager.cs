using System.Collections.Generic;
using UnityEngine;

public class ShopManager : LocalSingleton<ShopManager> , IInteractable
{
    [SerializeField] private List<ShopSlot> _shopSlots = new();
    [SerializeField] private List<SellableItem> _sellableItems = new();
    [SerializeField] private float _interactionTime = 0;
    public bool IsShopOpen { get; set; } = false;

    public void Init()
    {
        SetShopSlots();
    }
    private void Update()
    {
        if (IsShopOpen && Input.GetKeyUp(KeyCode.Escape))
        {
            OpenShop();
        }
    }
    private void OpenShop()
    {
        if (UpgradeManager.Instance.IsUpgradePanelOpen) return;
        
        if (!IsShopOpen)
        {
            UIManager.Instance.OpenShopPanel();
            UIManager.Instance.OpenInventoryPanel();
            UIManager.Instance.HideCrosshair();
            UIManager.Instance.HideInteractionText();
        }
        else
        {
            UIManager.Instance.CloseShopPanel();
            UIManager.Instance.CloseInventoryPanel();
            UIManager.Instance.ShowCrosshair();
        }
        
        GameManager.Instance.CheckCursorLockState();
    }
    private void SetShopSlots()
    {
        for (var i = 0; i < _shopSlots.Count; i++)
        {
            var shopSlot = _shopSlots[i];
            var sellableItem = _sellableItems[i];
            shopSlot.SetItemData(sellableItem.itemData);
            shopSlot.SetItemDisplayImage(sellableItem.itemData.thumbnail);
            shopSlot.SetItemNameText(sellableItem.itemData.name);
            shopSlot.UpdatePriceText();
        }
    }

    public void Interact(float interactionTime = 0)
    {
        OpenShop();
    }

    public string Text_OnAim() => "Press E to Interact";
    public float GetInteractionTime() => _interactionTime;
}

[System.Serializable]
public class SellableItem
{
    public ItemData itemData;
}