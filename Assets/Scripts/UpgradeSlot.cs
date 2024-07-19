using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UpgradeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Button _upgradeBtn;
    [SerializeField] private TextMeshProUGUI _upgradeBtnText;
    [SerializeField] private Image _itemDisplayImage; 
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _priceText;

    private UpgradableItem _upgradableItem;
    private int _slotIndex;
    
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.Instance.InventoryToHand(_slotIndex);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(_upgradableItem.upgradableItemData[_upgradableItem.upgradeLevel].itemData, false, true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(null);
    }

    public void UpgradeButton_OnClick()
    {
        if (CurrencyManager.Instance.GetCurrency() >= _upgradableItem.upgradableItemData[_upgradableItem.upgradeLevel].itemData.price)
        {
            CurrencyManager.Instance.DealCurrency(-_upgradableItem.upgradableItemData[_upgradableItem.upgradeLevel].itemData.price);
            
            UpgradeManager.Instance.UpgradeItem(_upgradableItem);
            
            UpdateUpgradeSlot();
            InventoryManager.Instance.SaveInventory();
        }
    }

    public void UpdatePriceText()
    {
        _priceText.text = "$" + UIManager.Instance.FormatFloatToReadableString(_upgradableItem.upgradableItemData[_upgradableItem.upgradeLevel].itemData.price);
    }

    public void SetItemNameText(string newName)
    {
        var formattedName = Regex.Replace(newName, "(?<!^)([A-Z])", " $1");
        _itemNameText.text = formattedName;
    }

    public void SetUpgradableItem(UpgradableItem newData)
    {
        _upgradableItem = newData;
        UpdateUpgradeSlot();
    }

    public void SetItemDisplayImage(Sprite newImage)
    {
        _itemDisplayImage.sprite = newImage;
    }

    public void OnItemMax()
    {
        _upgradeBtn.interactable = false;
        _upgradeBtnText.text = "MAX";
    }

    private void UpdateUpgradeSlot()
    {
        int displayLevel = Mathf.Clamp(_upgradableItem.upgradeLevel + 1, 0, _upgradableItem.upgradableItemData.Length - 1);

        SetItemDisplayImage(_upgradableItem.upgradableItemData[displayLevel].itemData.thumbnail);
        SetItemNameText(_upgradableItem.upgradableItemData[displayLevel].itemData.name);
        UpdatePriceText();

        if (_upgradableItem.upgradeLevel == _upgradableItem.upgradableItemData.Length - 1)
        {
            OnItemMax();
        }
    }

}
