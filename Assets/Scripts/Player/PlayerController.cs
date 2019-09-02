using System.Collections.Generic;
using System.Linq;
using GameCamera;
using NPC;
using UI.Hud;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    [RequireComponent(typeof(PlayerMotor))]
    public class PlayerController : MonoBehaviour
    {
        public LayerMask movementMask;
        public LayerMask interactableMask;
        public float rotationSpeed = 100f;
        
        private Interactable focus;
        private Camera cam;
        private CameraController _cameraController;
        private PlayerActor actor;
        private PlayerMotor playerMotor;
        private bool buttonStillDown = false;

        private void Awake()
        {
            cam = Camera.main;
            playerMotor = GetComponent<PlayerMotor>();
            _cameraController = cam.GetComponent<CameraController>();
            actor = GetComponent<PlayerActor>();
        }

        void Start()
        {
            actor.combat.TargetDied += OnTargetDied;
            
            PlayerManager.instance.UI.actionBar.onActionKeyClick += ActionKeyDown;
        }

        void Update()
        {
            if (IsPointerOverUIObject()) 
                return;

            if (Input.GetMouseButtonDown(0))
            {
                buttonStillDown = true;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                buttonStillDown = false;
            }
            
            /**
             * When you click on screen once "rotation" will be 0.3, -0.1 and etc,
             * but when you try to rotate camera "rotation" will be like 0.1245 and then we lock control
             */
//            float rotation = Input.GetAxis("Mouse X");
//            if (rotation != 0 && rotation.ToString().Length > 3)
//            {
//                return;
//            }

            if (buttonStillDown)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, 100, interactableMask))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    _cameraController.PointerFollow(hit.collider.gameObject);

                    if (interactable != null)
                    {
                        SetFocus(interactable);
                        return;
                    }
                }
                
                if (Physics.Raycast(ray, out hit, 100, movementMask))
                {
                    _cameraController.SetPointer(hit.point);

                    playerMotor.MoveTo(hit.point);
                    RemoveFocus();
                    return;
                }
            }
           
            if (Input.GetKeyDown(KeyCode.E))
            {
                ActionKeyDown();
            }
        }

        void SetFocus (Interactable newFocus)
        {
            if (focus != newFocus)
            {
                if (focus != null)
                {
                    focus.OnDisfocused();
                }
                this.focus = newFocus;
                playerMotor.Follow(focus);
            }
        
            focus.OnFocused(transform);
        }

        public void ActionKeyDown()
        {
            if (focus == null)
            {
                return;
            }

            if (focus.InInteracableDistance(transform))
            {
                focus.Interact();
            }
        }

        void OnTargetDied()
        {
            if (focus)
            {
                NPCActor npcActor = focus.GetComponent<NPCActor>();

                if (npcActor != null)
                {
                    actor.TargetDied(npcActor);
                }
            }
            
            RemoveFocus();
        }
        
        void RemoveFocus()
        {
            if (focus)
            {
                focus.OnDisfocused();
            }
            focus = null;
       
            playerMotor.StopFollow();
        }

        
        private bool IsPointerOverUIObject() {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
 
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

    }
}
