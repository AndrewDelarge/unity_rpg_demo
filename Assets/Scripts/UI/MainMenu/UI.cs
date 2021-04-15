using GameSystems;
using GameSystems.GameModes;
using Managers;
using UnityEngine;

namespace UI.MainMenu
{
    
    // TODO: Перенести в главный UI ?
    [RequireComponent(typeof(Loading))]
    public class UI : MonoBehaviour
    {

        public GameObject loading;

        private Loading _loading;
        
        private void Awake()
        {
            _loading = loading.GetComponent<Loading>();
            
            _loading.Hide();
        }

        public void OnButtonStart()
        {
            GameModeManager.Instance().SetGameState(GameModeManager.GameModeState.TO_GAME_FROM_MAIN_MENU);
        }

        public void OnButtonExit()
        {
            Application.Quit();
        }
    }
}
