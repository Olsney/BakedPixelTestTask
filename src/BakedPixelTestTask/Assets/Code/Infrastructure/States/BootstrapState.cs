using Code.Services.StaticData;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IStaticDataService _staticDataService;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, IStaticDataService staticDataService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            _staticDataService.LoadAllItems();
            _staticDataService.LoadAllInventoryConfigs();
            _staticDataService.LoadGameBalanceConfig();
            
            if (SceneManager.GetActiveScene().name == Initial)
            {
                EnterLoadLevel();
            }
            else
            {
                _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
            }
        }

        private void EnterLoadLevel() => 
            _stateMachine.Enter<LoadProgressState>();

        public void Exit()
        {
        }
    }
}