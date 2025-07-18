using System;
using Code.Gameplay.Items;
using Code.StaticData.Item;

namespace Code.Gameplay.Inventory
{
    public class SlotModel
    {
        private ItemModel _item;

        public ItemModel Item => _item;
        public bool IsEmpty => _item == null;

        public event Action ItemChanged;

        public void SetItem(ItemModel item)
        {
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
            if (!CanStack(config))
                return false;

            _item.Add(count);
            ItemChanged?.Invoke();
            
            return true;
        }
    }
}