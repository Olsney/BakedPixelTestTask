using System.Collections.Generic;
using Code.Services.PersistentProgress;
using UnityEngine;

namespace Code.Infrastructure.Factory.Game
{
    public interface IGameFactory
    {
        List<ISavedProgress> ProgressWriters { get; }
        List<ISavedProgressReader> ProgressReaders { get; }
        GameObject CreateHud();
        void Cleanup();
    }
}