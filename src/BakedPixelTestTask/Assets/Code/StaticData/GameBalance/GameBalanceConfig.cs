using UnityEngine;

namespace Code.StaticData.GameBalance
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game")]
    public class GameBalanceConfig : ScriptableObject
    {
        public int CoinsPerClick = 50;
        public int AmmoPerClick = 30;
        public string[] RandomItemIds;
    }
}