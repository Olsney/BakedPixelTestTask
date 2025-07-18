using UnityEngine;

[CreateAssetMenu(fileName = "InventoryConfig", menuName = "Configs/Inventory")]
public class InventoryConfig : ScriptableObject
{
    public InventoryId Id;
    public int Capacity;
}