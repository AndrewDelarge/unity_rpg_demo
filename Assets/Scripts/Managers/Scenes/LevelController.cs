using System;
using Gameplay;
using Gameplay.Player;
using GameSystems;
using UnityEngine;

namespace Managers.Scenes
{
    public class LevelController : MonoBehaviour
    {
        public AIActorsController actorsController;
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
            currentLevel = GameObject.CreatePrimitive(PrimitiveType.Plane);
            
            PrepareLevel();
        }
        
        public void LoadLevel(GameObject level)
        {
            if (currentLevel != null)
                UnloadLevel();
            
            currentLevel = Instantiate(level);

            PrepareLevel();
        }
        
        public GameObject FindSpawnPoint(int spawnPointId)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].id == spawnPointId)
                    return spawnPoints[i].gameObject;
            }

            return null;
        }

        public int GetStartSpawnId()
        {
            if (levelSettings != null)
                return levelSettings.spawnPointId;

            return 0;
        }

        private void UnloadLevel()
        {
            if (currentLevel == null)
                return;
            
            OnLevelUnload?.Invoke();
            Destroy(currentLevel);
            currentLevel = null;
        }

        private void PrepareLevel()
        {
            if (currentLevel == null)
            {
                Debug.Log(" # -LvlCntr- # Current Level Not Found or not loaded");
                return;
            }
            
            actorsController = new AIActorsController();
            triggersManager = new TriggersManager(FindObjectsOfType<Trigger>());
            
            spawnPoints = FindObjectsOfType<SpawnPoint>();
            levelSettings = FindObjectOfType<LevelSettings>();
            
            triggersManager.Init();

            if (levelSettings != null)
            {
                PlayerManager.Instance().TeleportToPoint(levelSettings.spawnPointId);
                levelSettings.Apply();
            }
            
            OnLevelLoaded?.Invoke();
        }
    }
}