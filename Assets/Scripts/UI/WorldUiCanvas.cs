﻿using UnityEngine;

namespace UI
{
    public class WorldUiCanvas : MonoBehaviour
    {
        public GameObject dialogBalloons;
        public GameObject healthbars;


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
                default:
                    targetToSpawn = this.gameObject;
                    break;
            }

            return Instantiate(gameObject, targetToSpawn.transform);
        }
        
    }

    public enum WorldUiType
    {
        Dialog, Healthbars
    }
}