using System;

namespace Code.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public int Coins { get; private set; }
        
        public void AddCoins(int amount)
        {
            if(amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            
            Coins += amount;
        }
        
        public void Pay(int amount)
        {
            if(amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            
            Coins -= amount;
        }
    }
}