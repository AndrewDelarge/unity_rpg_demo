using System;
using Actors.Player;
using Exceptions.Game.Player;
using Gameplay;
using Gameplay.Player;
using Managers.Player;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public GameObject playerPrefab;

        [TagSelector]
        public string spawnTag;
        
        private PlayerActor currentPlayer;
        [HideInInspector]
        public EquipmentManager equipmentManager;
        [HideInInspector]
        public InventoryManager inventoryManager;
        public SpawnPoint[] spawnPoints;

        public Action onPlayerInited;
        
        public void Init()
        {
            spawnPoints = FindObjectsOfType<SpawnPoint>();
            equipmentManager = GetComponent<EquipmentManager>();
            inventoryManager = GetComponent<InventoryManager>();
            equipmentManager.Init();
            inventoryManager.Init();
        }
        
        public GameObject SpawnPlayer(int spawnPointId)
        {
            if (currentPlayer != null)
            {
                return currentPlayer.gameObject;
            }
            
            GameObject point = FindSpawnPoint(spawnPointId);

            if (point == null)
            {
                return null;
                throw new CouldntSpawnPlayer();
            }
            
            GameObject currentPlayerGO = Instantiate(playerPrefab, point.transform.position, point.transform.rotation);

            currentPlayer = currentPlayerGO.GetComponent<PlayerActor>();
            
            equipmentManager.targetMesh = currentPlayer.gameObject;
            equipmentManager.EquipDefault();
            Destroy(point);
            return currentPlayer.gameObject;
        }

        public void InitPlayer()
        {
            currentPlayer.Init();
            onPlayerInited?.Invoke();
        }
        
        GameObject FindSpawnPoint(int spawnPointId)
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

        public void TeleportToPoint(int spawnPointId)
        {
            GameObject point = FindSpawnPoint(spawnPointId);
            if (point != null)
            {
                ChangePlayerPos(point.transform.position);
            }
        }
        
        public PlayerActor GetPlayer()
        {
            return currentPlayer;
        }

        private void ChangePlayerPos(Vector3 pos)
        {
            currentPlayer.transform.position = pos;
        }
    }
}