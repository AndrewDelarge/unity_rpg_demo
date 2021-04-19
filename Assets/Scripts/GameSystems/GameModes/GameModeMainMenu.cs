using Managers;
using UnityEngine;

namespace GameSystems.GameModes
{
    public class GameModeMainMenu : GameMode
    {

        protected override void GameStateChanged(GameModeManager.GameModeState state)
        {
            switch (state)
            {
                case GameModeManager.GameModeState.TO_MAIN_MENU_FROM_LOADER:
                    ToMainMenuFromLoader();
                    break;
                case GameModeManager.GameModeState.IN_MAIN_MENU:
                    InMainMenu();
                    break;
            }
        }

        

        private void ToMainMenuFromLoader()
        {
            GameManager.Instance().LoadMainMenu();
        }


        private void InMainMenu()
        {
            UIManager.Instance().SetScreen(UIManager.UIScreens.MainMenu);
            CameraManager.Instance().SwitchCamera(GameCameras.Menu);
            // In main menu state logic
        }
    }
}