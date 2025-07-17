using Code.Data;
using Code.Services.PersistentProgress;
using Code.Services.SaveLoad;

namespace Code.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private const string MainSceneName = "Main";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadProgress;

        public LoadProgressState(GameStateMachine gameStateMachine, 
            IPersistentProgressService progressService, 
            ISaveLoadService saveLoadProgress)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadProgress = saveLoadProgress;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            
            _gameStateMachine.Enter<LoadLevelState, string>(MainSceneName);
        }

        public void Exit()
        {
        }
        
        private void LoadProgressOrInitNew()
        {
            _progressService.Progress = 
                _saveLoadProgress.LoadProgress() 
                ?? NewProgress();
        }
        
        private PlayerProgress NewProgress()
        {
            var progress =  new PlayerProgress();
            
            return progress;
        }
    }
}