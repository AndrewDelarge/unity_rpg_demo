using System.Collections.Generic;
using System.Linq;
using GameCamera;
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
        
        
        private PlayerMotor playerMotor;

       
        void Start()
        {
            cam = Camera.main;
            playerMotor = GetComponent<PlayerMotor>();
            _cameraController = cam.GetComponent<CameraController>();
            PlayerManager.instance.UI.actionBar.onActionKeyClick += ActionKeyDown;
        }

        void Update()
        {
            if (IsPointerOverUIObject()) 
                return;

//            float rotation = Input.GetAxis("Mouse X");
            
            /**
             * When you click on screen once "rotation" will be 0.3, -0.1 and etc,
             * but when you try to rotate camera "rotation" will be like 0.1245 and then we lock control
             */
//            if (rotation != 0 && rotation.ToString().Length > 3)
//            {
//                return;
//            }


            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, 100, interactableMask))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    _cameraController.SetPointer(hit.collider.transform.position);

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
                }
            }
           
            if (Input.GetKeyDown(KeyCode.E))
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
