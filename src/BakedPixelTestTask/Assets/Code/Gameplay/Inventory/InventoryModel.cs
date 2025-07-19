using System;
using System.Collections.Generic;
using Code.Data;
using Code.Gameplay.Items;
using Code.Services.PersistentProgress;
using Code.Services.StaticData;
using Code.StaticData.Item;

namespace Code.Gameplay.Inventory
{
    public class InventoryModel : ISavedProgress
    {
        private readonly IStaticDataService _staticData;

        private List<SlotModel> _slots;
        private int _unlockPrice;


        public List<SlotModel> Slots => new(_slots);
        public int Capacity => _slots.Count;

        public event Action InventoryChanged;
        
        public int UnlockSlotPrice => _unlockPrice;

        public InventoryModel(IStaticDataService staticData)
        {
            _staticData = staticData;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            InventoryConfig inventoryConfig = _staticData.GetInventoryConfig(InventoryId.Player);

            _unlockPrice = inventoryConfig.UnlockSlotPrice;

            if (progress.Inventory == null)
                progress.Inventory = new InventoryProgress();
            
            if (progress.Inventory.Slots == null || progress.Inventory.Slots.Length != inventoryConfig.Capacity)
            {
                progress.Inventory.Slots = new SlotProgress[inventoryConfig.Capacity];

                for (int i = 0; i < inventoryConfig.Capacity; i++)
                {
                    progress.Inventory.Slots[i] = new SlotProgress
                    {
                        IsLocked = i >= inventoryConfig.DefaultUnlockedSlots
                    };
                }
            }
            
            _slots = new List<SlotModel>(inventoryConfig.Capacity);

            for (int i = 0; i < inventoryConfig.Capacity; i++)
            {
                SlotProgress slotData = progress.Inventory.Slots[i];
                var slot = new SlotModel(slotData.IsLocked);

                if (!string.IsNullOrEmpty(slotData.ItemId))
                {
                    ItemConfig item = _staticData.GetItemConfig(slotData.ItemId);
                    
                    if (item != null)
                        slot.SetItem(new ItemModel(item, slotData.Count));
                }

                slot.ItemChanged += OnSlotChanged;
                
                _slots.Add(slot);
            }

            InventoryChanged?.Invoke();
        }
        
        public void UpdateProgress(PlayerProgress progress)
        {
            if (progress.Inventory == null)
                progress.Inventory = new InventoryProgress();

            if (progress.Inventory.Slots == null || progress.Inventory.Slots.Length != _slots.Count)
                progress.Inventory.Slots = new SlotProgress[_slots.Count];

            for (int i = 0; i < _slots.Count; i++)
            {
                SlotModel slot = _slots[i];
                SlotProgress data = progress.Inventory.Slots[i] ?? new SlotProgress();
                data.IsLocked = slot.IsLocked;

                if (slot.IsEmpty)
                {
                    data.ItemId = null;
                    data.Count = 0;
                }
                else
                {
                    data.ItemId = slot.Item.Config.Id;
                    data.Count = slot.Item.Count;
                }

                progress.Inventory.Slots[i] = data;
            }
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

        public void RemoveItem(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Count)
                return;

            SlotModel slot = _slots[slotIndex];

            if (slot.IsEmpty)
                return;

            slot.Clear();
            InventoryChanged?.Invoke();
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