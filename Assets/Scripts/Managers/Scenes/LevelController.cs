using System;
using Gameplay;
using Gameplay.Player;
using GameSystems;
using UnityEngine;

namespace Managers.Scenes
{
    public class LevelController : MonoBehaviour
    {
        public AIActorsManager actorsManager;
        public event Action OnLevelLoaded;
        public event Action OnLevelUnload;
        
        
        private TriggersManager triggersManager;
        private SpawnPoint[] spawnPoints;
        private LevelSettings levelSettings;
        private GameObject currentLevel;

        public void Init()
        {
        }

        public void StartCurrentEditorLevel()
        {
            currentLevel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            PrepareLevel();
        }
        
        public void LoadLevel(GameObject level)
        {
            if (currentLevel != null)
            {
                UnloadLevel();
            }
            currentLevel = Instantiate(level);

            PrepareLevel();
        }
        
        public GameObject FindSpawnPoint(int spawnPointId)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].id == spawnPointId)
                {
                    return spawnPoints[i].gameObject;
                }
            }

            return null;
        }

        public int GetStartSpawnId()
        {
            if (levelSettings != null)
            {
                return levelSettings.spawnPointId;
            }

            return 0;
        }

        private void UnloadLevel()
        {
            if (currentLevel == null)
            {
                return;
            }
            OnLevelUnload?.Invoke();
            Destroy(currentLevel);
            currentLevel = null;
        }

        private void PrepareLevel()
        {
            actorsManager = new AIActorsManager();
            triggersManager = new TriggersManager(FindObjectsOfType<Trigger>());
            
            spawnPoints = FindObjectsOfType<SpawnPoint>();
            levelSettings = FindObjectOfType<LevelSettings>();
            
            triggersManager.Init();

            if (levelSettings != null)
            {
                GameController.instance.playerManager.TeleportToPoint(levelSettings.spawnPointId);
                levelSettings.Apply();
            }
            
            OnLevelLoaded?.Invoke();
        }
    }
}