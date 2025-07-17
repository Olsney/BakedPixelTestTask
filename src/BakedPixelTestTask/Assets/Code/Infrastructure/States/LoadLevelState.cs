using Code.Infrastructure.Factory.Game;
using Code.Services.PersistentProgress;
using Code.UI.Services.Factory;
using UnityEngine;

namespace Code.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string EmptySceneName = "Empty";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IPersistentProgressService _persistentProgressService;

        public LoadLevelState(
            GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            IGameFactory gameFactory,
            IUIFactory uiFactory,
            IPersistentProgressService persistentProgressService
        )
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _persistentProgressService = persistentProgressService;
        }

        public void Enter(string sceneName)
        {
            _gameFactory.Cleanup();
            _sceneLoader.Load(EmptySceneName, () => _sceneLoader.Load(sceneName, OnLoaded));
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            InitUIRoot();
            InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
        }

        private void LoadRequstedScene(string sceneName)
        {
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        private void InitGameWorld()
        {
            GameObject gameObject = _gameFactory.CreateHud();
        }

        private void InitUIRoot()
        {
            _uiFactory.CreateUIRoot();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_persistentProgressService.Progress);

        }
    }
}