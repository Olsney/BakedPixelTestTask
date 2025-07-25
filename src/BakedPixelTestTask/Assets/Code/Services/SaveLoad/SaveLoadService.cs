﻿using System.IO;
using Code.Data;
using Code.Gameplay.Inventory;
using Code.Infrastructure.Factory.Game;
using Code.Services.PersistentProgress;
using UnityEngine;

namespace Code.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressFileName = "progress.json";

        private string ProgressFilePath =>
            Path.Combine(Application.persistentDataPath, ProgressFileName);
        
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        private readonly InventoryModel _inventoryModel;

        public SaveLoadService(IPersistentProgressService progressService, 
            IGameFactory gameFactory,
            InventoryModel inventoryModel)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
            _inventoryModel = inventoryModel;
        }

        public void SaveProgress()
        {
            foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);
         
            _inventoryModel.UpdateProgress(_progressService.Progress);
            
            string json = _progressService.Progress.ToJson();
            File.WriteAllText(ProgressFilePath, json);
        }

        public PlayerProgress LoadProgress()
        {
            if (!File.Exists(ProgressFilePath))
            {
                Debug.LogWarning("Progress file not found, returning default progress.");
                return new PlayerProgress();
            }

            string json = File.ReadAllText(ProgressFilePath);
            
            return string.IsNullOrEmpty(json)
                ? new PlayerProgress()
                : json.ToDeserialized<PlayerProgress>();
        }
    }
}