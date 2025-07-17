using UnityEngine;

namespace Code.StaticData.Item
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Configs/Item")]
    public class ItemConfig : ScriptableObject
    {
        [Header("General")]
        public string Id;
        public string DisplayName;
        public Sprite Icon;
        public ItemType Type;
        public ItemCategory Category;
        public float Weight = 1f;
        public int MaxStack = 1;

        [Header("Weapon")]
        public int Damage;
        public ItemType AmmoType;

        [Header("Armor")]
        public int Defense;
    }
}