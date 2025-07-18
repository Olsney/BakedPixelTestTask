using Code.Gameplay.Inventory;
using Code.Infrastructure.Factory.AssetManagement;
using Code.UI.Presenters;
using Code.UI.View;
using UnityEngine;
using Zenject;

namespace Code.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assets;
        private readonly InventoryModel _inventoryModel;

        private Transform _uiRoot;

        public UIFactory(IInstantiator instantiator,
            IAssetProvider assets,
            InventoryModel inventoryModel)
        {
            _instantiator = instantiator;
            _assets = assets;
            _inventoryModel = inventoryModel;
        }

        public void CreateUIRoot()
        {
            if (_uiRoot != null)
                return;

            GameObject prefab = _assets.Load(AssetPath.UIRootPath);
            _uiRoot = _instantiator.InstantiatePrefab(prefab).transform;
        }

        public void CreatePresenters(HudView hudView)
        {
            CreateInventoryPresenter(hudView);
        }

        private InventoryPresenter CreateInventoryPresenter(HudView hudView)
        {
            _inventoryModel.Initialize();

            return new InventoryPresenter(_inventoryModel, hudView.InventoryView);
        }
    }
}