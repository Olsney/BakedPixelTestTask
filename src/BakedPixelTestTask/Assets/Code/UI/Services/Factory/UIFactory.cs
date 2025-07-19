using System;
using System.Collections.Generic;
using Code.Gameplay.Inventory;
using Code.Infrastructure.Factory.AssetManagement;
using Code.Services.PersistentProgress;
using Code.Services.StaticData;
using Code.UI.Presenters;
using Code.UI.View;
using UnityEngine;
using Zenject;

namespace Code.UI.Services.Factory
{
    public class UIFactory : IUIFactory, IDisposable
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assets;
        private readonly InventoryModel _inventoryModel;
        private readonly IPersistentProgressService _progress;
        private readonly IStaticDataService _staticData;


        private Transform _uiRoot;
        private List<IDisposable> _presenters = new();

        public UIFactory(IInstantiator instantiator,
            IAssetProvider assets,
            InventoryModel inventoryModel, 
            IPersistentProgressService progress, 
            IStaticDataService staticData)
        {
            _instantiator = instantiator;
            _assets = assets;
            _inventoryModel = inventoryModel;
            _progress = progress;
            _staticData = staticData;
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
            _presenters.Add(CreateInventoryPresenter(hudView));
            _presenters.Add(CreateHudPresenter(hudView));
        }

        private InventoryPresenter CreateInventoryPresenter(HudView hudView)
        {
            _inventoryModel.LoadProgress(_progress.Progress);

            return new InventoryPresenter(_inventoryModel, hudView.InventoryView);
        }
        
        private HudPresenter CreateHudPresenter(HudView hudView)
        {
            HudPresenter hudPresenter = new HudPresenter(_inventoryModel, _progress, _staticData, hudView);
            
            hudPresenter.Initialize();

            return hudPresenter;
        }

        public void Dispose()
        {
            foreach (IDisposable disposable in _presenters)
            {
                disposable.Dispose();
            }
        }
    }
}