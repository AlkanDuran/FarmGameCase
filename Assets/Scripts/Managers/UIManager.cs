using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using UnityEngine.Serialization;

public class UIManager : LocalSingleton<UIManager>, ITimeTracker
{
    [Header("Status Bar")]

    [SerializeField] private TextMeshProUGUI toolQuantityText;
    [SerializeField] private Image _itemEquipSlot;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _dateText; 
    
    [Header("Inventory System")]
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private HandInventorySlot _handSlot; 
    [SerializeField] private InventorySlot[] _itemSlots;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText; 
    [SerializeField] private TextMeshProUGUI _itemPriceText;
    [Space] 
    [SerializeField] private Image _crosshairImage;
    [SerializeField] private TextMeshProUGUI _interactionText;
    [SerializeField] private YesNoPrompt yesNoPrompt;
    [SerializeField] private Transform _inventoryImageTransform;
    [SerializeField] private Transform _tutorialHandSpriteTransform;
    
    public void Init()
    {
        RenderInventory();
        AssignSlotIndexes();
        TimeManager.Instance.RegisterTracker(this);
        TimeManager.Instance.Init();
    }
    private void AssignSlotIndexes()
    {
        for (var i =0; i<_itemSlots.Length; i++)
        {
            _itemSlots[i].AssignIndex(i);
        }
    }
    public void RenderInventory()
    {

        ItemSlotData[] inventoryItemSlots = InventoryManager.Instance.GetInventorySlots();

        
        RenderInventoryPanel(inventoryItemSlots, _itemSlots);
        _handSlot.Display(InventoryManager.Instance.GetEquippedSlot());
        
        var equippedItem = InventoryManager.Instance.GetEquippedSlotItem();

        toolQuantityText.text = "";
        
        if (equippedItem != null)
        {
            _itemEquipSlot.sprite = equippedItem.thumbnail;

            _itemEquipSlot.gameObject.SetActive(true);

            int quantity = InventoryManager.Instance.GetEquippedSlot().quantity;
            if(quantity>1) toolQuantityText.text = quantity.ToString();
            
            return;
        }

        _itemEquipSlot.gameObject.SetActive(false);
    }

    private void RenderInventoryPanel(ItemSlotData[] slots, InventorySlot[] uiSlots)
    {
        int length = Mathf.Min(slots.Length, uiSlots.Length);
        for (var i = 0; i < length; i++)
        {
            uiSlots[i].Display(slots[i]);
        }

        for (var i = length; i < uiSlots.Length; i++)
        {
            uiSlots[i].Display(null);
        }
    }

    public void OpenInventoryPanel()
    {
        var inventoryRect = _inventoryPanel.GetComponent<RectTransform>();
        var newOffsetMin = inventoryRect.offsetMin;
        var newOffsetMax = inventoryRect.offsetMax;

        InventoryManager.Instance.IsInventoryOpen = true;
        _inventoryPanel.SetActive(true);
        
        if (ShopManager.Instance.IsShopOpen || UpgradeManager.Instance.IsUpgradePanelOpen)
        {
            newOffsetMin.x = 950f;
            newOffsetMax.x = 0f;
            inventoryRect.offsetMin = newOffsetMin;
            inventoryRect.offsetMax = newOffsetMax;
        }
        else
        {
            newOffsetMin.x = 0f;
            newOffsetMax.x = 0f;
            inventoryRect.offsetMin = newOffsetMin;
            inventoryRect.offsetMax = newOffsetMax;
        }
        RenderInventory();
    }

    public void CloseInventoryPanel()
    {
        _inventoryPanel.SetActive(false);
        InventoryManager.Instance.IsInventoryOpen = false;
    }

    public void OpenShopPanel()
    {
        _shopPanel.SetActive(true);
        ShopManager.Instance.IsShopOpen = true;
    }

    public void CloseShopPanel()
    {
        _shopPanel.SetActive(false);
        ShopManager.Instance.IsShopOpen = false;
    }

    public void OpenUpgradePanel()
    {
        UpgradeManager.Instance.IsUpgradePanelOpen = true;
        _upgradePanel.SetActive(true);
    }

    public void CloseUpgradePanel()
    {
        UpgradeManager.Instance.IsUpgradePanelOpen = false;
        _upgradePanel.SetActive(false);
    }
    public void DisplayItemInfo(ItemData data, bool isBoughtable = false, bool isUpgradable = false)
    {
        if(data == null)
        {
            _itemNameText.text = "";
            _itemDescriptionText.text = "";
            _itemPriceText.text = "";
            return;
        }

        _itemNameText.text = data.name;
        _itemDescriptionText.text = data.description;
        if (ShopManager.Instance.IsShopOpen && data.itemType == ItemType.Sellable)
        {
            if(isBoughtable) _itemPriceText.text = "<color=green>Buy Price:</color> " + data.price.ToString();
            else if(isUpgradable) _itemPriceText.text = "<color=green>Upgrade Price:</color> " + data.price.ToString();
            else _itemPriceText.text = "<color=green>Sell Price:</color> " + data.price.ToString();
        }
    }
    public void ClockUpdate(GameTimestamp timestamp)
    {
        var hours = timestamp.hour;
        var minutes = timestamp.minute; 
        
        _timeText.text =hours + ":" + minutes.ToString("00");
        
        var day = timestamp.day;
        var season = timestamp.season.ToString();
        var dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();
        
        _dateText.text = season + " " + day + " (" + dayOfTheWeek +")";
    }
    public string FormatFloatToReadableString(float value, bool isDecimal = false, bool isTwoDigit = false)
    {
        var number = value;
        var hasDecimalPart = (number % 1) != 0;

        if(number < 100f && isDecimal && hasDecimalPart)
        {
            return number.ToString(isTwoDigit ? "0.0" : "0.00").Replace(",",".");
        }
        if (number < 1000)
        {
            return ((int)number).ToString();
        }
        var result = ((int)number).ToString();
        if (result.Contains(","))
        {
            result = result.Substring(0, 4);
            result = result.Replace(",", string.Empty);
        }
        else
        {
            result = result.Substring(0, 3);
        }
        do
        {
            number /= 1000;
        }
        while (number >= 1000);

        number = (int)number;
        if (value >= 1000000000000000)
        {
            result = result + "Q";
        }
        else if (value >= 1000000000000)
        {
            result = result + "T";
        }
        else if (value >= 1000000000)
        {
            result = result + "B";
        }
        else if (value >= 1000000)
        {
            result = result + "M";
        }
        else if (value >= 1000)
        {
            result = result + "K";
        }
        if (((int)number).ToString().Length > 0 && ((int)number).ToString().Length < 3)
        {
            result = result.Insert(((int)number).ToString().Length, ".");
        }
        return result;
        



    }
    public Vector2 GetCanvasPositionFromWorldPosition(Vector3 worldPos, RectTransform canvasRect)
    {
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldPos);
        Vector2 worldObjectScreenPos = new Vector2(
            ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

        return worldObjectScreenPos;
    }

    public void TriggerYesNoPrompt(string message, System.Action onYesCallBack)
    {
        yesNoPrompt.ShowPrompt(true);
        GameManager.Instance.CheckCursorLockState();
        yesNoPrompt.CreatePrompt(message, onYesCallBack);
    }

    public GameObject GetInventoryPanel() => _inventoryPanel;
    public void ShowCrosshair() => _crosshairImage.enabled = true;
    public void HideCrosshair() => _crosshairImage.enabled = false;
    public void ShowInteractionText() => _interactionText.enabled = true;
    public void HideInteractionText() => _interactionText.enabled = false;
    public TextMeshProUGUI GetInteractionText() => _interactionText;
    public Transform GetInventoryImageTransform() => _inventoryImageTransform;
    public InventorySlot[] GetInventorySlots() => _itemSlots;
    public Transform GetTutorialHandSpriteTransform() => _tutorialHandSpriteTransform;
}