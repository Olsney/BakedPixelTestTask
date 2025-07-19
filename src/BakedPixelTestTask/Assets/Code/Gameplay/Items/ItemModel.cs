using System;
using Code.StaticData.Item;
using UnityEngine;

namespace Code.Gameplay.Items
{
    public class ItemModel
    {
        private int _count;
        public event Action Changed;

        public ItemConfig Config { get; }
        public int Count => _count;

        public ItemModel(ItemConfig config, int count = 1)
        {
            Config = config;
            _count = count;

            Changed?.Invoke();
        }

        public float TotalWeight => Config.Weight * Count;
        public bool IsStackable => Config.MaxStack > 1;

        public void Add(int amount)
        {
            _count += amount;
            
            Changed?.Invoke();
        }

        public void Remove(int amount)
        {
            _count -= amount;
            
            Changed?.Invoke();

            if (_count < 0)
                _count = 0;
        }

        public void Set(int amount) =>
            _count = Mathf.Max(0, amount);
    }
}