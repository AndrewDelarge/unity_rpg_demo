using System;
using GameSystems;
using UnityEngine;

namespace UI
{
    public class WorldUiCanvas : MonoBehaviour
    {
        public GameObject dialogBalloons;
        public GameObject healthbars;

        public WorldUiObjects worldUiObjects;

        public GameObject SpawnUi(GameObject gameObject, WorldUiType type)
        {

            GameObject targetToSpawn;
            
            switch (type)
            {
                case WorldUiType.Dialog:
                    targetToSpawn = dialogBalloons;
                    break;
                    
                case WorldUiType.Healthbars:
                    targetToSpawn = healthbars;
                    break;
                // TODO make go for damage feed in user hud
                case WorldUiType.DamageFeed:
                    targetToSpawn = GameController.instance.uiManager.GetHudGameObject();
                    break;
                default:
                    targetToSpawn = this.gameObject;
                    break;
            }

            return Instantiate(gameObject, targetToSpawn.transform);
        }
        
        
        
    }

    [Serializable]
    public struct WorldUiObjects
    {
        public GameObject damageTextFeed;
    }
    
    public enum WorldUiType
    {
        Dialog, Healthbars, DamageFeed
    }
}
