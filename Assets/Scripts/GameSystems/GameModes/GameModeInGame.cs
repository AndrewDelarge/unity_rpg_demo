using Managers;
using UnityEngine;

namespace GameSystems.GameModes
{
    public class GameModeInGame : GameMode
    {

        protected override void GameStateChanged(GameModeManager.GameModeState state)
        {
            switch (state)
            {
                case GameModeManager.GameModeState.TO_GAME_FROM_MAIN_MENU:
                    ToGameFromMainMenu();
                    break;
                case GameModeManager.GameModeState.IN_GAME:
                    InGame();
                    break;
                case GameModeManager.GameModeState.DEVELOPMENT_SCENE:
                    InGame();
                    break;
                
            }
        }

        private void ToGameFromMainMenu()
        {
            SceneController sceneController = GameManager.Instance().sceneController;

            sceneController.LoadScene(SceneController.BaseScenes.Indexes.GAME);
            sceneController.OnSceneLoaded += () =>
                GameModeManager.Instance().SetGameState(GameModeManager.GameModeState.IN_GAME);
        }

        private void InGame()
        {
            PlayerManager playerManager = PlayerManager.Instance();
            SceneController sceneController = GameManager.Instance().sceneController;
            
            playerManager.SpawnPlayer(sceneController.GetStartSpawnPoint());
            var player = playerManager.CurrentPlayer;
            
            CameraManager.Instance().SwitchCamera(GameCameras.InGame);
            CameraManager.Instance().target = player.transform;

            UIManager.Instance().SetScreen(UIManager.UIScreens.InGameHud);
            playerManager.InitPlayer();
            
            sceneController.ApplySceneSettings();
            
            GameManager.Instance().Pause(false);
        }

    }
}