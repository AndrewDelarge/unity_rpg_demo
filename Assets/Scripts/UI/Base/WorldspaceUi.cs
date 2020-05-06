using System.Collections;
using Exceptions.Game.UI;
using GameSystems;
using UnityEngine;

namespace UI.Base
{
    public class WorldspaceUi : HideableUi
    {
        
        public GameObject uiPrefab;
        public Transform target;
        public bool scaleFromParent = true;
        public WorldUiType type;
        
        protected CameraController cam;
        protected WorldUiCanvas worldUiCanvas;
        protected float scale = 1;
        
        
        
        public override void Init()
        {
            cam = GameController.instance.mainCamera;
            worldUiCanvas = GameController.instance.worldUiCanvas;
            scale = 1;

            curElement = worldUiCanvas.SpawnUi(uiPrefab, type);
            if (curElement == null)
            {
                throw new WorldspaceUiSpawnException();
            }

            if (scaleFromParent)
            {
                scale = transform.localScale.x;
            }
            
            if (target == null)
            {
                target = transform;
                // Try to up over head
                scale *= 2.3f;
            }
            
            curElement.SetActive(false);
        }
        
        void FixedUpdate()
        {
            curElement.transform.position = new Vector3(target.position.x, target.position.y + (scale), target.position.z);
            curElement.transform.forward = -cam.GetCamera().transform.forward;
        }
    }
}