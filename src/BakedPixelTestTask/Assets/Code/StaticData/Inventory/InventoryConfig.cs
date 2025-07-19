using UnityEngine;

[CreateAssetMenu(fileName = "InventoryConfig", menuName = "Configs/Inventory")]
public class InventoryConfig : ScriptableObject
{
    public InventoryId Id;
    public int Capacity;
    
    [Header("Unlocking")]
    public int DefaultUnlockedSlots = 15;
    public int UnlockSlotPrice = 10;
}