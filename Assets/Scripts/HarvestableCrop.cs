using UnityEngine;

public class HarvestableCrop : MonoBehaviour, IInteractable
{
    [SerializeField] private float _interactionTime;
    public ItemData item;
    public void Interact(float interactionTime)
    {
        InventoryManager.Instance.HandToInventory();
        InventoryManager.Instance.EquipHandSlot(item);
        var cropBehaviour = GetComponentInParent<CropBehaviour>();
        cropBehaviour.RemoveCrop();
        
        gameObject.SetActive(false);
    }

    public string Text_OnAim()
    {
        return "Press E to Harvest";
    }

    public float GetInteractionTime() => _interactionTime;
}
