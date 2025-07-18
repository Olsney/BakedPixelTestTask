using System.Collections.Generic;
using Code.StaticData.Item;

namespace Code.Services.StaticData
{
    public interface IStaticDataService
    {
        void LoadAllItems();
        ItemConfig GetItemConfig(string id);
        IReadOnlyList<ItemConfig> AllItems { get; }
        void LoadAllInventoryConfigs();
        InventoryConfig GetInventoryConfig(InventoryId id);
    }
}