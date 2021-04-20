using System;
using CoreUtils;
using Exceptions.Game.Player;
using Gameplay;
using Gameplay.Actors.Player;
using Gameplay.Player;
using GameSystems;
using Managers.Player;
using Managers.Scenes;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : SingletonDD<PlayerManager>
    {
        public GameObject playerPrefab;
        public EquipmentManager equipmentManager;
        public InventoryManager inventoryManager;
        public Action onPlayerInited;

        private SceneController sceneController;
        private PlayerActor currentPlayer;
        public PlayerActor CurrentPlayer => currentPlayer;

        public void Init()
        {
            sceneController = GameManager.Instance().sceneController;
            equipmentManager.Init();
            inventoryManager.Init();
        }
        
        public void SpawnPlayer(int spawnPointId)
        {
            if (currentPlayer != null)
                return;

            GameObject point = sceneController.FindSpawnPoint(spawnPointId);

            if (point == null)
                throw new CouldntSpawnPlayer();
            
            GameObject currentPlayerGO = Instantiate(playerPrefab, point.transform.position, point.transform.rotation);

            currentPlayer = currentPlayerGO.GetComponent<PlayerActor>();
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
                ChangePlayerPos(point.transform.position);
        }
        
        // TODO remove
        public PlayerActor GetPlayer() => CurrentPlayer;

        private void ChangePlayerPos(Vector3 pos)
        {
            if (currentPlayer == null)
            {
                Debug.Log(" # -PlayerMng- # Player not spawned");
                return;
            }
            currentPlayer.transform.position = pos;
        }
    }
}