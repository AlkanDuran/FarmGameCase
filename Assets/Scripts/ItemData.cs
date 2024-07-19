using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName ="Items/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string description;
    public Sprite thumbnail;
    public GameObject gameModel;
    public int price;
    public int slotIndex;
}

public enum ItemType { Sellable, Upgradable } 