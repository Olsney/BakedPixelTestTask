using Code.Infrastructure.Factory.Game;
using Code.Infrastructure.States;
using Code.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private GameStateMachine _gameStateMachine;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine,
            IGameFactory gameFactory,
            ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _saveLoadService = saveLoadService;
        }
        
        private void Start()
        {
            _gameStateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}