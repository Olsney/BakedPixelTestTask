using System;
using Code.Gameplay.Items;
using Code.StaticData.Item;

namespace Code.Gameplay.Inventory
{
    public class SlotModel
    {
        private ItemModel _item;
        private bool _locked;


        public ItemModel Item => _item;
        public bool IsEmpty => _item == null;
        public bool IsLocked => _locked;

        public event Action ItemChanged;
        
        public SlotModel(bool locked = false)
        {
            _locked = locked;
        }

        public void SetItem(ItemModel item)
        {
            if (_locked)
                return;
            
            _item = item;
            
            ItemChanged?.Invoke();
        }

        public void Clear()
        {
            _item = null;
            ItemChanged?.Invoke();
        }

        public bool CanStack(ItemConfig config)
        {
            return _item != null &&
                   _item.Config.Id == config.Id &&
                   _item.IsStackable;
        }

        public bool TryStack(ItemConfig config, int count)
        {
            if (_locked || !CanStack(config))
                return false;

            _item.Add(count);
            ItemChanged?.Invoke();
            
            return true;
        }
        
        public bool Unlock()
        {
            if (!_locked)
                return false;

            _locked = false;
            ItemChanged?.Invoke();
            
            return true;
        }
    }
}