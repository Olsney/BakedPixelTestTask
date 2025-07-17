using UnityEngine;

namespace Code.Infrastructure.Factory.Game
{
    public interface IGameFactory
    {
        GameObject CreateHud();
    }
}