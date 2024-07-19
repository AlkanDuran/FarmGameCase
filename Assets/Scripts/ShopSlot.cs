using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image _itemDisplayImage; 
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _priceText;

    private ItemData _itemData;
    private int _slotIndex;
    
    public virtual void OnPointerClick(PointerEventData eventData)
    {
       
        InventoryManager.Instance.InventoryToHand(_slotIndex);
    }
    
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(_itemData, true);
    }

   
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(null);
    }

    public void BuyButton_OnClick()
    {
        if (CurrencyManager.Instance.GetCurrency() >= _itemData.price)
        {
            CurrencyManager.Instance.DealCurrency(-_itemData.price);
            InventoryManager.Instance.DirectToInventory(_itemData);
        }
    }

    public void UpdatePriceText()
    {
        _priceText.text = "$" + UIManager.Instance.FormatFloatToReadableString(_itemData.price);
    }
    public void SetItemNameText(string newName)
    {
        var formattedName = Regex.Replace(newName, "(?<!^)([A-Z])", " $1");
        _itemNameText.text = formattedName;
    }
    public void SetItemData(ItemData newData) => _itemData = newData;
    public void SetItemDisplayImage(Sprite newImage) => _itemDisplayImage.sprite = newImage;
}