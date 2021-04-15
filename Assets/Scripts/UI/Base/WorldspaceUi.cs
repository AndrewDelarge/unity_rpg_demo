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
        
        protected CameraManager cam;
        protected WorldUiCanvas worldUiCanvas;
        protected float scale = 1;
        
        
        public override void Init()
        {
            cam = CameraManager.Instance();
            worldUiCanvas = GameManager.Instance().sceneController.worldUiCanvas;
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

            inited = true;
            curElement.SetActive(false);
        }
        
        void FixedUpdate()
        {
            if (!inited)
            {
                return;
            }

            curElement.transform.position = new Vector3(target.position.x, target.position.y + (scale), target.position.z);
            curElement.transform.forward = - cam.GetCamera().transform.forward;
        }
    }
}