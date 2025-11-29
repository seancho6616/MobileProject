using UnityEngine;

[CreateAssetMenu(fileName = "ItemState", menuName = "Scriptable Objects/ItemState")]
public class ItemState : ScriptableObject
{
    public enum Item{ Coin, Potion, Heart, Dis_Heart, Stamina}
    public Item itemType;
    public string itemName;
    public float coinValue;
    public float value;
}
