using System.Collections.Generic;
using System.Linq;
using Code.StaticData.Item;
using UnityEngine;

namespace Code.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string ItemsPath = "Configs/Items";
        
        private Dictionary<string, ItemConfig> _items;

        public IReadOnlyList<ItemConfig> AllItems => _items.Values.ToList();

        public void LoadItems()
        {
            _items = Resources
                .LoadAll<ItemConfig>(ItemsPath)
                .ToDictionary(x => x.Id, x => x);
        }

        public ItemConfig ForItem(string id)
        {
            return _items.TryGetValue(id, out var config)
                ? config
                : null;
        }
    }
}