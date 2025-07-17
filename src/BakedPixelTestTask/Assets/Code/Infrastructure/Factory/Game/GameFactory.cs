using System.Collections.Generic;
using Code.Infrastructure.Factory.AssetManagement;
using Code.Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Factory.Game
{
    public class GameFactory : IGameFactory
    {
        private readonly IInstantiator _container;
        private readonly IAssetProvider _assets;
        
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();

        public GameFactory(IInstantiator container, 
            IAssetProvider assets,
            IPersistentProgressService progressService)
        {
            _container = container;
            _assets = assets;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HudPath);
            
            return hud;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject prefab = _assets.Load(path: prefabPath);
            
            GameObject instance = _container.InstantiatePrefab(prefab);
            RegisterProgressWatchers(instance);

            return instance;
        }

        private void RegisterProgressWatchers(GameObject instance)
        {
            foreach (ISavedProgressReader progressReader in instance.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if(progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);
            
            ProgressReaders.Add(progressReader);
        }
    }
}