using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : LocalSingleton<UpgradeManager>, IInteractable
{
    [SerializeField] private List<UpgradeSlot> _upgradeSlots = new();
    [SerializeField] private List<UpgradableItem> _upgradableItems = new();
    [SerializeField] private float _interactionTime = 0;
    public bool IsUpgradePanelOpen { get; set; } = false;

    public void Init()
    {
        SetUpgradeSlots();
    }

    private void Update()
    {
        if (IsUpgradePanelOpen && Input.GetKeyUp(KeyCode.Escape))
        {
            OpenUpgradePanel();
        }
    }

    public void OpenUpgradePanel()
    {
        if (ShopManager.Instance.IsShopOpen) return;
        
        if (!IsUpgradePanelOpen)
        {
            UIManager.Instance.OpenUpgradePanel();
            UIManager.Instance.OpenInventoryPanel();
            UIManager.Instance.HideCrosshair();
            UIManager.Instance.HideInteractionText();
        }
        else
        {
            UIManager.Instance.CloseUpgradePanel();
            UIManager.Instance.CloseInventoryPanel();
            UIManager.Instance.ShowCrosshair();
        }
        
        GameManager.Instance.CheckCursorLockState();
    }
    
    public void SetUpgradeSlots()
    {
        for (var i = 0; i < _upgradeSlots.Count; i++)
        {
            var upgradeSlot = _upgradeSlots[i];
            var upgradableItem = _upgradableItems[i];

            upgradableItem.SetUpgradeLevel();
            int displayLevel = Mathf.Clamp(upgradableItem.upgradeLevel + 1, 0, upgradableItem.upgradableItemData.Length - 1);

            upgradeSlot.SetUpgradableItem(upgradableItem);
            upgradeSlot.SetItemDisplayImage(upgradableItem.upgradableItemData[displayLevel].itemData.thumbnail);
            upgradeSlot.SetItemNameText(upgradableItem.upgradableItemData[displayLevel].itemData.name);
            upgradeSlot.UpdatePriceText();
        }
    }


    public void UpgradeItem(UpgradableItem item)
    {
        if (item.upgradeLevel < item.upgradableItemData.Length - 1)
        {
            var previousItem = item.upgradableItemData[item.upgradeLevel].itemData;
            
            foreach (var slot in InventoryManager.Instance.GetInventorySlots())
            {
                if (slot.itemData == previousItem)
                {
                    InventoryManager.Instance.ConsumeItem(slot);
                    break;
                }
            }

            if (InventoryManager.Instance.GetEquippedSlot().itemData == previousItem)
            {
                InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot());
            }
                
            
            item.IncreaseUpgradeLevel();
            var newItem = item.upgradableItemData[item.upgradeLevel].itemData;
            InventoryManager.Instance.DirectToInventory(newItem);
        }
        SetUpgradeSlots();
    }

    public void Interact(float interactionTime = 0)
    {
        OpenUpgradePanel();
    }

    public string Text_OnAim() => "Press E to Interact";
    public float GetInteractionTime() => _interactionTime;
}

[System.Serializable]
public class UpgradableItem
{
    public int upgradeLevel = 0;
    public string upgradeLevelPlayerPrefName;
    public UpgradableItemData[] upgradableItemData;

    public void SetUpgradeLevel()
    {
        upgradeLevel = PlayerPrefs.GetInt(upgradeLevelPlayerPrefName, 0);
        upgradeLevel = Mathf.Clamp(upgradeLevel, 0, upgradableItemData.Length - 1);
    }

    public void IncreaseUpgradeLevel()
    {
        upgradeLevel++;
        upgradeLevel = Mathf.Clamp(upgradeLevel, 0, upgradableItemData.Length - 1);
        PlayerPrefs.SetInt(upgradeLevelPlayerPrefName, upgradeLevel);
    }
}

[System.Serializable]
public class UpgradableItemData
{
    public ItemData itemData;
}
