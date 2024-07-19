using System;
using UnityEngine;
public class InventoryManager : LocalSingleton<InventoryManager>
{
    [Header("Tools")]
    [SerializeField] private ItemSlotData[] _itemSlots = new ItemSlotData[10];
    [SerializeField] private ItemSlotData _equippedItemSlot = null; 
    [SerializeField] private Transform _handPoint;

    public ItemIndex itemIndex;
    private GameObject _equippedItem;

    public bool IsInventoryOpen { get; set; } = false;

    public void Init()
    {
        LoadInventory();
    }
    private void OnApplicationQuit()
    {
        SaveInventory();
    }
    private void Update()
    {
        if (IsInventoryOpen && Input.GetKeyUp(KeyCode.Escape))
        {
            OpenInventory();
            return;
        }
        if (Input.GetKeyUp(GameManager.Instance.GetPcKeyBindData().GetInventoryKey()))
        {
            OpenInventory();
        }
    }
    public void OpenInventory()
    {
        if (UpgradeManager.Instance.IsUpgradePanelOpen || ShopManager.Instance.IsShopOpen || SettingsPanel.Instance.isSettingsOpen) return;
        
        if (!IsInventoryOpen)
        {
            UIManager.Instance.OpenInventoryPanel();
            UIManager.Instance.HideCrosshair();
            UIManager.Instance.HideInteractionText();
        }
        else
        {
            UIManager.Instance.CloseInventoryPanel();
            UIManager.Instance.ShowCrosshair();
        }
        GameManager.Instance.CheckCursorLockState();
    }
    public void InventoryToHand(int slotIndex)
    {

        ItemSlotData handToEquip = _equippedItemSlot;
        ItemSlotData[] inventoryToAlter = _itemSlots;
        
        if(handToEquip.Stackable(inventoryToAlter[slotIndex]))
        {
            ItemSlotData slotToAlter = inventoryToAlter[slotIndex];

            handToEquip.AddQuantity(slotToAlter.quantity);
            slotToAlter.Empty();
        }
        else
        {
            ItemSlotData slotToEquip = new ItemSlotData(inventoryToAlter[slotIndex]);

            inventoryToAlter[slotIndex] = new ItemSlotData(handToEquip);
            EquipHandSlot(slotToEquip);
            
        }

        RenderHand();
        UIManager.Instance.RenderInventory();

    }
    public void HandToInventory()
    {

        ItemSlotData handSlot = _equippedItemSlot;
        ItemSlotData[] inventoryToAlter = _itemSlots;

        if(!StackItemToInventory(handSlot, inventoryToAlter))
        { 
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    inventoryToAlter[i] = new ItemSlotData(handSlot);
                    handSlot.Empty();
                    break;
                }
            }
        }
        RenderHand();
        UIManager.Instance.RenderInventory();
    }

    public void DirectToInventory(ItemData itemData)
    {
        ItemSlotData newItemSlot = new ItemSlotData(itemData);
        if (!StackItemToInventory(newItemSlot, _itemSlots))
        {
            for (int i = 0; i < _itemSlots.Length; i++)
            {
                if (_itemSlots[i].IsEmpty())
                {
                    _itemSlots[i] = new ItemSlotData(newItemSlot);
                    break;
                }
            }
        }
        RenderHand();
        UIManager.Instance.RenderInventory();
    }

    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        for(int i = 0; i < inventoryArray.Length; i++)
        {
            if(inventoryArray[i].Stackable(itemSlot))
            {
                inventoryArray[i].AddQuantity(itemSlot.quantity);
                itemSlot.Empty();
                return true;
            }
        }
        return false;
    }
    public void RenderHand()
    {  
        if(_handPoint.childCount > 0)
        {
            Destroy(_handPoint.GetChild(0).gameObject);
        }
        
        if(SlotEquipped())
        {
            _equippedItem = Instantiate(GetEquippedSlotItem().gameModel, _handPoint);
        }
    }
    public ItemData GetEquippedSlotItem() => _equippedItemSlot.itemData;
    public ItemSlotData GetEquippedSlot() => _equippedItemSlot;
    public ItemSlotData[] GetInventorySlots() =>  _itemSlots;

    public bool SlotEquipped() => !_equippedItemSlot.IsEmpty();
    public void EquipHandSlot(ItemData item) 
    {
        _equippedItemSlot = new ItemSlotData(item);
        RenderHand();
        UIManager.Instance.RenderInventory();
    }

    public void EquipHandSlot(ItemSlotData itemSlot) 
    {
        ItemData item = itemSlot.itemData;
        _equippedItemSlot = new ItemSlotData(itemSlot);
    }
    
    private void OnValidate()
    {
        ValidateInventorySlot(_equippedItemSlot);
        ValidateInventorySlots(_itemSlots);
    }

    void ValidateInventorySlot(ItemSlotData slot)
    {
        if(slot.itemData !=null && slot.quantity ==0)
        {
            slot.quantity = 1;
        }
    }

    void ValidateInventorySlots(ItemSlotData[] array)
    {
        foreach (ItemSlotData slot in array)
        {
            ValidateInventorySlot(slot);
        }
    }

    public void ConsumeItem(ItemSlotData itemSlot)
    {
        if(itemSlot.IsEmpty())
        {
            return;
        }
        itemSlot.Remove();
        RenderHand();
        UIManager.Instance.RenderInventory();
    }
    public void SaveInventory()
    {
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            PlayerPrefs.SetString($"InventorySlot_{i}_Item", _itemSlots[i].itemData != null ? _itemSlots[i].itemData.name : string.Empty);
            PlayerPrefs.SetInt($"InventorySlot_{i}_Quantity", _itemSlots[i].quantity);
        }

        if (_equippedItemSlot != null && _equippedItemSlot.itemData != null)
        {
            PlayerPrefs.SetString("EquippedItem", _equippedItemSlot.itemData.name);
            PlayerPrefs.SetInt("EquippedItem_Quantity", _equippedItemSlot.quantity);
        }
        else
        {
            PlayerPrefs.SetString("EquippedItem", string.Empty);
            PlayerPrefs.SetInt("EquippedItem_Quantity", 0);
        }

        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            string itemName = PlayerPrefs.GetString($"InventorySlot_{i}_Item", _itemSlots[i].itemData != null ? _itemSlots[i].itemData.name : string.Empty);
            int quantity = PlayerPrefs.GetInt($"InventorySlot_{i}_Quantity", _itemSlots[i].quantity != 0 ? _itemSlots[i].quantity : 0);
            if (!string.IsNullOrEmpty(itemName) && quantity > 0)
            {
                ItemData itemData = itemIndex.GetItemFromString(itemName);
                _itemSlots[i] = new ItemSlotData(itemData, quantity);
            }
            else
            {
                _itemSlots[i] = new ItemSlotData();
            }
        }

        string equippedItemName = PlayerPrefs.GetString("EquippedItem", string.Empty);
        int equippedItemQuantity = PlayerPrefs.GetInt("EquippedItem_Quantity", 0);

        if (!string.IsNullOrEmpty(equippedItemName) && equippedItemQuantity > 0)
        {
            ItemData equippedItemData = itemIndex.GetItemFromString(equippedItemName);
            _equippedItemSlot = new ItemSlotData(equippedItemData, equippedItemQuantity);
        }
        else
        {
            _equippedItemSlot = new ItemSlotData();
        }

        RenderHand();
        UIManager.Instance.RenderInventory();
    }

    public GameObject GetEquippedItem() => _equippedItem;

}