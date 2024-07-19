[System.Serializable]
public class ItemSlotData
{
    public ItemData itemData;
    public int quantity;
    
    public ItemSlotData(ItemData itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
        ValidateQuantity();
    }
    public ItemSlotData(ItemData itemData)
    {
        this.itemData = itemData;
        quantity = 1;
        ValidateQuantity();
    }
    public ItemSlotData (ItemSlotData slotToClone)
    {
        itemData = slotToClone.itemData;
        quantity = slotToClone.quantity; 
    }
    public ItemSlotData()
    {
        Empty();
    }
    public void AddQuantity()
    {
        AddQuantity(1); 
    }
    public void AddQuantity(int amountToAdd)
    {
        quantity += amountToAdd; 
    }

    public void Remove()
    {
        quantity--;
        ValidateQuantity();
    }
    public bool Stackable(ItemSlotData slotToCompare)
    {
        return slotToCompare.itemData == itemData; 
    }
    private void ValidateQuantity()
    {
        if (quantity <= 0 || itemData == null)
        {
            Empty();
        }
    }
    public void Empty()
    {
        itemData = null;
        quantity = 0;
    }
    public bool IsEmpty()
    {
        return itemData == null; 
    }
}