using Code.Infrastructure.Factory.AssetManagement;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Factory.Game
{
    public class GameFactory : IGameFactory
    {
        private readonly IInstantiator _container;
        private readonly IAssetProvider _assets;

        public GameFactory(IInstantiator container, IAssetProvider assets)
        {
            _container = container;
            _assets = assets;
        }

        public GameObject CreateHud()
        {
            GameObject prefab = _assets.Load(AssetPath.HudPath);
            
            GameObject hud = _container.InstantiatePrefab(prefab);

            return hud;
        }
    }
}