using System;

namespace Code.Data
{
    [Serializable]
    public class SlotProgress
    {
        public bool IsLocked;
        public string ItemId;
        public int Count;
    }
}