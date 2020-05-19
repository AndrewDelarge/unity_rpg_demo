using System;
using Actors.Player;
using Exceptions.Game.Player;
using Gameplay;
using Gameplay.Player;
using GameSystems;
using Managers.Player;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        [HideInInspector]
        public EquipmentManager equipmentManager;
        [HideInInspector]
        public InventoryManager inventoryManager;
        public Action onPlayerInited;

        private SceneController sceneController;
        private PlayerActor currentPlayer;
        
        public void Init()
        {
            equipmentManager = GetComponent<EquipmentManager>();
            inventoryManager = GetComponent<InventoryManager>();
            sceneController = GameController.instance.sceneController;
            equipmentManager.Init();
            inventoryManager.Init();
        }
        
        public GameObject SpawnPlayer(int spawnPointId)
        {
            if (currentPlayer != null)
            {
                return currentPlayer.gameObject;
            }
            
            GameObject point = sceneController.FindSpawnPoint(spawnPointId);

            if (point == null)
            {
                throw new CouldntSpawnPlayer();
            }
            
            GameObject currentPlayerGO = Instantiate(playerPrefab, point.transform.position, point.transform.rotation);

            currentPlayer = currentPlayerGO.GetComponent<PlayerActor>();
            
            equipmentManager.targetMesh = currentPlayer.gameObject;
            equipmentManager.EquipDefault();
            return currentPlayer.gameObject;
        }

        public void InitPlayer()
        {
            currentPlayer.Init();
            onPlayerInited?.Invoke();
        }
        


        public void TeleportToPoint(int spawnPointId)
        {
            GameObject point = sceneController.FindSpawnPoint(spawnPointId);
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