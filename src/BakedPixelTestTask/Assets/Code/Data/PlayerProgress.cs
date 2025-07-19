using System;
using UnityEngine.Serialization;

namespace Code.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public event Action<int> CoinsChanged;
        public InventoryProgress Inventory;

        public PlayerProgress()
        {
            Inventory = new InventoryProgress();
        }

        public int Coins { get; private set; }
        
        public void AddCoins(int amount)
        {
            if(amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            
            Coins += amount;
            CoinsChanged?.Invoke(Coins);
        }
        
        public void Pay(int amount)
        {
            if(amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            
            Coins -= amount;
            CoinsChanged?.Invoke(Coins);
        }
    }
}