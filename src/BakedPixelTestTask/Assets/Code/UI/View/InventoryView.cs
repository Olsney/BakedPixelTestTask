using System.Collections.Generic;
using UnityEngine;

namespace Code.UI.View
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private List<SlotView> _slots;

        public int SlotCount => _slots.Count;

        public SlotView GetSlotView(int index)
        {
            if (index < 0 || index >= _slots.Count)
                return null;

            return _slots[index];
        }
    }
}