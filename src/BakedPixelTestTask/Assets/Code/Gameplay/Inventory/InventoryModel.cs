using System;
using System.Collections.Generic;
using Code.Gameplay.Items;
using Code.StaticData.Item;

namespace Code.Gameplay.Inventory
{
    public class InventoryModel
    {
        private readonly List<SlotModel> _slots;

        public IReadOnlyList<SlotModel> Slots => new List<SlotModel>(_slots);
        public int Capacity => _slots.Count;

        public event Action OnInventoryChanged;

        public InventoryModel(int initialSlotCount)
        {
            _slots = new List<SlotModel>(initialSlotCount);
            
            for (int i = 0; i < initialSlotCount; i++)
                _slots.Add(new SlotModel());
        }
        
        public float GetTotalWeight()
        {
            float total = 0f;
                
            for (int i = 0; i < _slots.Count; i++)
            {
                if (!_slots[i].IsEmpty)
                    total += _slots[i].Item.TotalWeight;
            }
                
            return total;
        }

        public bool TryAddItem(ItemConfig config, int count = 1)
        {
            if (config.MaxStack > 1)
            {
                for (int i = 0; i < _slots.Count; i++)
                {
                    SlotModel slot = _slots[i];
                    if (slot.CanStack(config))
                    {
                        slot.TryStack(config, count);
                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                }
            }

            for (int i = 0; i < _slots.Count; i++)
            {
                SlotModel slot = _slots[i];
                if (slot.IsEmpty)
                {
                    slot.SetItem(new ItemModel(config, count));
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }

            return false;
        }

        public bool RemoveItem(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Count)
                return false;

            SlotModel slot = _slots[slotIndex];
            if (slot.IsEmpty)
                return false;

            slot.Clear();
            OnInventoryChanged?.Invoke();
            return true;
        }

        public void AddEmptySlots(int count)
        {
            for (int i = 0; i < count; i++)
                _slots.Add(new SlotModel());

            OnInventoryChanged?.Invoke();
        }

        public List<int> FindAllOccupiedSlotIndexes()
        {
            List<int> result = new List<int>();
            for (int i = 0; i < _slots.Count; i++)
            {
                if (!_slots[i].IsEmpty)
                    result.Add(i);
            }

            return result;
        }
    }
}
