using System;
using System.Collections.Generic;
using Code.Gameplay.Items;
using Code.Services.StaticData;
using Code.StaticData.Item;

namespace Code.Gameplay.Inventory
{
    public class InventoryModel
    {
        private readonly IStaticDataService _staticData;

        private List<SlotModel> _slots;
        private int _unlockPrice;


        public List<SlotModel> Slots => new List<SlotModel>(_slots);
        public int Capacity => _slots.Count;

        public event Action InventoryChanged;
        
        public int UnlockSlotPrice => _unlockPrice;
        public int UnlockedSlotsCount
        {
            get
            {
                int count = 0;
                foreach (var slot in _slots)
                    if (!slot.IsLocked)
                        count++;
                return count;
            }
        }
        public bool HasLockedSlots
        {
            get
            {
                foreach (var slot in _slots)
                    if (slot.IsLocked)
                        return true;
                return false;
            }
        }

        public InventoryModel(IStaticDataService staticData)
        {
            _staticData = staticData;
        }

        public void Initialize()
        {
            InventoryConfig inventoryConfig = _staticData.GetInventoryConfig(InventoryId.Player);

            _unlockPrice = inventoryConfig.UnlockSlotPrice;

            _slots = new List<SlotModel>(inventoryConfig.Capacity);

            for (int i = 0; i < inventoryConfig.Capacity; i++)
            {
                bool locked = i >= inventoryConfig.DefaultUnlockedSlots;
                var slot = new SlotModel(locked);
                slot.ItemChanged += OnSlotChanged;
                _slots.Add(slot);
            }

            InventoryChanged?.Invoke();
        }
        
        private void OnSlotChanged() =>
            InventoryChanged?.Invoke();

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
            return TryAddItem(config, count, out _);
        }

        public bool TryAddItem(ItemConfig config, int count, out List<int> affectedSlots)
        {
            affectedSlots = new List<int>();
            
            if (config.MaxStack > 1)
            {
                for (int i = 0; i < _slots.Count && count > 0; i++)
                {
                    SlotModel slot = _slots[i];
                    
                    
                    if (slot.IsLocked)
                        continue;

                    if (slot.CanStack(config))
                    {
                        int add = Math.Min(count, config.MaxStack - slot.Item.Count);
                        slot.TryStack(config, add);
                        count -= add;
                        
                        affectedSlots.Add(i);
                    }
                }
            }

            for (int i = 0; i < _slots.Count && count > 0; i++)
            {
                SlotModel slot = _slots[i];
                
                if (slot.IsLocked)
                    continue;
                
                if (slot.IsEmpty)
                {
                    int add = Math.Min(count, config.MaxStack);
                    
                    slot.SetItem(new ItemModel(config, add));
                    
                    count -= add;
                    affectedSlots.Add(i);
                }
            }

            if (affectedSlots.Count > 0)
                InventoryChanged?.Invoke();

            return count == 0;
        }

        public bool RemoveItem(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Count)
                return false;

            SlotModel slot = _slots[slotIndex];

            if (slot.IsEmpty)
                return false;

            slot.Clear();
            InventoryChanged?.Invoke();

            return true;
        }

        public void AddEmptySlots(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var slot = new SlotModel();
                slot.ItemChanged += OnSlotChanged;
                _slots.Add(slot);
            }

            InventoryChanged?.Invoke();
        }
        
        public bool UnlockNextSlot()
        {
            foreach (var slot in _slots)
            {
                if (slot.IsLocked)
                {
                    slot.Unlock();
                    InventoryChanged?.Invoke();
                    return true;
                }
            }

            return false;
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