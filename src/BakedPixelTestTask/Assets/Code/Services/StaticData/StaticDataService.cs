using System.Collections.Generic;
using System.Linq;
using Code.StaticData.GameBalance;
using Code.StaticData.Item;
using UnityEngine;

namespace Code.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string ItemsPath = "Configs/Items";
        private const string GameConfigPath = "Configs/Game/GameConfig";
        
        private Dictionary<string, ItemConfig> _items;
        private Dictionary<InventoryId, InventoryConfig> _inventoryConfigs;
        private GameBalanceConfig _gameBalanceConfig;


        public IReadOnlyList<ItemConfig> AllItems => _items.Values.ToList();

        public void LoadAllItems()
        {
            _items = Resources
                .LoadAll<ItemConfig>(ItemsPath)
                .ToDictionary(x => x.Id, x => x);
        }

        public ItemConfig GetItemConfig(string id)
        {
            return _items.TryGetValue(id, out var config)
                ? config
                : null;
        }

        public void LoadAllInventoryConfigs()
        {
            _inventoryConfigs = Resources
                .LoadAll<InventoryConfig>("Configs/Inventories")
                .ToDictionary(x => x.Id, x => x);
        }

        public InventoryConfig GetInventoryConfig(InventoryId id)
        {
            return _inventoryConfigs.TryGetValue(id, out var config) 
                ? config 
                : throw new KeyNotFoundException($"No InventoryConfig for id {id}");        
        }

        public void LoadGameBalanceConfig() => 
            _gameBalanceConfig = Resources.Load<GameBalanceConfig>(GameConfigPath);

        public GameBalanceConfig GetGameBalanceConfig() =>
            _gameBalanceConfig;
    }
}