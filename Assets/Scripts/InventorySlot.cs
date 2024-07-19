using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private ItemData _itemData;
    private int _quantity;
    private int _slotIndex; 
    
    public Image itemDisplayImage; 
    public TextMeshProUGUI quantityText;

    public enum InventoryType
    {
        Item, Tool
    }
    
    public void Display(ItemSlotData itemSlot)
    {

        _itemData = itemSlot.itemData;
        _quantity = itemSlot.quantity;
        quantityText.text = "";
       
        if(_itemData != null)
        {
            
            itemDisplayImage.sprite = _itemData.thumbnail;
           
            if(_quantity>1)quantityText.text = _quantity.ToString();
            itemDisplayImage.gameObject.SetActive(true);

            return; 
        }

        itemDisplayImage.gameObject.SetActive(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
  
        if (ShopManager.Instance.IsShopOpen && _itemData.itemType is ItemType.Sellable)
        {
            SellItem();
            return;
        }
        InventoryManager.Instance.InventoryToHand(_slotIndex);
    }


    public void AssignIndex(int slotIndex)
    {
        this._slotIndex = slotIndex;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(_itemData);
    }

  
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(null);
    }

    public void SellItem()
    {
        if (_itemData != null)
        {
            CurrencyManager.Instance.DealCurrency(_itemData.price);

            ItemSlotData itemSlotData = null;

            foreach (var slot in InventoryManager.Instance.GetInventorySlots())
            {
                if (slot.itemData == _itemData)
                {
                    itemSlotData = slot;
                }
            }
            InventoryManager.Instance.ConsumeItem(itemSlotData);
        }
    }
    public int GetSlotIndex() => _slotIndex;
}