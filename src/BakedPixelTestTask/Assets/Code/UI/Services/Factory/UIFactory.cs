using Code.Infrastructure.Factory.AssetManagement;
using UnityEngine;
using Zenject;

namespace Code.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assets;
        
        private Transform _uiRoot;

        public UIFactory(IInstantiator instantiator,
            IAssetProvider assets)
        {
            _instantiator = instantiator;
            _assets = assets;
        }
        
        public void CreateUIRoot()
        {
            if (_uiRoot != null)
                return;
            
            GameObject prefab = _assets.Load(AssetPath.UIRootPath);
            _uiRoot = _instantiator.InstantiatePrefab(prefab).transform;
        }
    }
}