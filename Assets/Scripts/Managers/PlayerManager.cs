using Actors.Player;
using Exceptions.Game.Player;
using Managers.Player;
using Player;
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


        public void Init()
        {
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
        }
        
        
        
        GameObject FindSpawnPoint(int spawnPointId)
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag(spawnTag);

            if (gos.Length < spawnPointId + 1 || gos.Length == 0)
            {
                return null;
            }
            
            return gos[spawnPointId];
        }


        public PlayerActor GetPlayer()
        {
            return currentPlayer;
        }

        public void ChangePlayerPos(Vector3 pos)
        {
            currentPlayer.transform.position = pos;
        }
    }
}