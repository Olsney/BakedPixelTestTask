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
            
            if(_item != null)
                _item.Changed -= OnItemChanged;
            
            _item = item;
            
            if(_item != null)
                _item.Changed += OnItemChanged;
            
            ItemChanged?.Invoke();
        }

        public void Clear()
        {
            if (_item != null)
                _item.Changed -= OnItemChanged;
            
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
        
        public void Unlock()
        {
            if (!_locked)
                return;

            _locked = false;
            ItemChanged?.Invoke();
        }
        
        private void OnItemChanged() =>
            ItemChanged?.Invoke();
    }
}