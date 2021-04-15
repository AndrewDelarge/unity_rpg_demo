using System;
using System.Collections.Generic;
using CoreUtils;
using UnityEngine;

namespace GameSystems.GameModes
{
    public class GameModeManager : SingletonDD<GameModeManager>
    {
        public enum GameModeState
        {
            NONE,
            IN_MAIN_MENU,
            TO_MAIN_MENU_FROM_LOADER,
            TO_GAME_FROM_MAIN_MENU,
            IN_GAME,
            TO_NEXT_LEVEL,
            PAUSE,
            DEVELOPMENT_SCENE
        }
        
        public Action<GameModeState> OnGameStateChangeAction = (GameModeState obj) => { };
        
        private static GameMode None = new GameMode();
        private static GameMode MainMenu = new GameModeMainMenu();
        private static GameMode InGame = new GameModeInGame();


        private Dictionary<CoreData.GameModeID, GameMode> _availableGameModes = new Dictionary<CoreData.GameModeID, GameMode>
        {
            { CoreData.GameModeID.None, None },
            { CoreData.GameModeID.MainMenu, MainMenu },
            { CoreData.GameModeID.Game, InGame }
        };

        private GameModeState _currentGameState = GameModeState.NONE;
        
        public GameModeState State => _currentGameState;

        private CoreData.GameModeID _activeGameMode = CoreData.GameModeID.None;
        
        public virtual GameMode ActiveGameMode 
        {
            get { return _availableGameModes[_activeGameMode]; }
        }

        private void ActivateGameMode(CoreData.GameModeID gameMode) 
        {
            _availableGameModes[_activeGameMode].Deactivate();
            _activeGameMode = gameMode;
            _availableGameModes[_activeGameMode].Activate();
        }

        private void GameStateChanged()
        {
            switch (_currentGameState)
            {
                case GameModeState.TO_MAIN_MENU_FROM_LOADER:
                    ActivateGameMode(CoreData.GameModeID.MainMenu);
                    break;
                case GameModeState.DEVELOPMENT_SCENE:
                case GameModeState.TO_GAME_FROM_MAIN_MENU:
                    ActivateGameMode(CoreData.GameModeID.Game);
                    break;
            }

            OnGameStateChangeAction(State);
        }
        
        
        public void SetGameState(GameModeState newState)
        {
            if (_currentGameState == newState)
                return;
            
            Debug.Log($"## Changing game state from: {_currentGameState} to: {newState} in progress ##");

            switch (newState)
            {
                case GameModeState.NONE:
                    break;
                case GameModeState.TO_MAIN_MENU_FROM_LOADER:
                    break;
                case GameModeState.PAUSE:
                    break;
                case GameModeState.TO_GAME_FROM_MAIN_MENU:
                    break;
                case GameModeState.TO_NEXT_LEVEL:
                    break;
                case GameModeState.IN_MAIN_MENU:
                    break;
                case GameModeState.IN_GAME:
                    if (! CanInGame)
                        return;
                    break;
            }

            _currentGameState = newState;
            
            GameStateChanged();
        }


        private bool CanInGame => _currentGameState == GameModeState.TO_GAME_FROM_MAIN_MENU;
    }
}