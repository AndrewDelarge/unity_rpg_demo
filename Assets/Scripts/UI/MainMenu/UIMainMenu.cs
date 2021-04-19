using GameSystems;
using GameSystems.GameModes;
using Managers;
using UI.Base;
using UI.Hud;
using UnityEngine;

namespace UI.MainMenu
{
    public class UIMainMenu : UIView
    {

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
