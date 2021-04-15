using UnityEngine;

namespace GameSystems.GameModes
{
    public class GameMode
    {

        protected virtual void GameStateChanged(GameModeManager.GameModeState state)
        {
            switch (state)
            {
                default:
                    Debug.LogWarning("Game is currently in -NONE- game mode!");
                    break;
            }
        }

        public GameMode() => Init();

        protected virtual void Init()
        {
            return;
        }

        public virtual void Activate()
        {
            GameModeManager.Instance().OnGameStateChangeAction += GameStateChanged;
        }

        public virtual void Deactivate()
        {
            GameModeManager.Instance().OnGameStateChangeAction -= GameStateChanged;
        }
    }
}