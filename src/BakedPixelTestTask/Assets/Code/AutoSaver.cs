using Code.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace Code
{
    public class AutoSaver : MonoBehaviour
    {
        private ISaveLoadService _saveLoadService;

        [Inject]
        public void Construct(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }
        
        private void OnApplicationPause(bool pause)
        {
            if (pause)
                _saveLoadService.SaveProgress();
        }

        private void OnApplicationQuit()
        {
            Debug.Log("Progress saved in ApplicationQuit");
            
            _saveLoadService.SaveProgress();
        }
    }
}