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
        private CharacterController _characterController;


        public FixedJoystick joystick;
        private Vector3 startPos;
        private Vector3 endPos;


        public float viewRadius;
        public float viewAngle;

        
        public List<Transform> visibleTargets = new List<Transform>(); 
        
        void FindVisibleTargets()
        {
            visibleTargets.Clear();
            
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, interactableMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    visibleTargets.Add(target);

//                    float dstToTarget = Vector3.Distance(target.position, transform.position);
//
//                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, interactableMask))
//                    {
//                    }
                    
                }

            }
        }

        public Vector3 DirFromAngle(float angleInDegrese, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrese += transform.eulerAngles.y;
            }
            Vector3 result = new Vector3(Mathf.Sin(angleInDegrese * Mathf.Deg2Rad), 0,
                Mathf.Cos(angleInDegrese * Mathf.Deg2Rad));
            Debug.Log(result);
            return result;
        }
        
        
        private void Awake()
        {
            cam = Camera.main;
            playerMotor = GetComponent<PlayerMotor>();
            _cameraController = cam.GetComponent<CameraController>();
            _characterController = GetComponent<CharacterController>();
            actor = GetComponent<PlayerActor>();
        }

        void Start()
        {
            actor.combat.TargetDied += OnTargetDied;
            
            PlayerManager.instance.UI.actionBar.onActionKeyClick += ActionKeyDown;
        }

        void Update()
        {
//            if (IsPointerOverUIObject()) 
//                return;
//
//            if (Input.GetMouseButtonDown(0))
//            {
//                buttonStillDown = true;
//            }
//            
//            if (Input.GetMouseButtonUp(0))
//            {
//                buttonStillDown = false;
//            }

            

            float horz = 0f;
            float vert = 0f;


            if (joystick != null)
            {
                horz = joystick.Horizontal;
                vert = joystick.Vertical;
            }

//            if (Input.GetMouseButtonDown(0))
//            {
//                startPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y) * 0.05f;
//
//            }
//
//            if (Input.GetMouseButton(0))
//            {
//                buttonStillDown = true;
//                endPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y) * 0.05f;
//            }
//            else
//            {
//                buttonStillDown = false;
//            }
//
//            if (buttonStillDown)
//            {
//                Vector2 offset = endPos - startPos;
//                
//                
//                Vector2 dir = Vector2.ClampMagnitude(offset, 1f);
//                    
//                
//                horz = dir.x;
//                vert = dir.y;
////                _characterController.Move(dir * -1);
//            }



            if (horz == 0f && vert == 0f)
            {
                horz = Input.GetAxis("Horizontal");
                vert = Input.GetAxis("Vertical");
            }
//            
//            
            Vector3 newPosition = Vector3.zero;

            if ((horz != 0f || vert != 0f) && _characterController.isGrounded)
            {
                
                newPosition = new Vector3(horz, 0f, vert);
                newPosition = cam.transform.TransformDirection(newPosition);
                newPosition.y = 0;
                newPosition = newPosition.normalized;
                Debug.Log(newPosition);
                newPosition *= 5.0f;


                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(newPosition.x, 0f, newPosition.z));
         
                Quaternion newRotation = Quaternion.Lerp(gameObject.transform.rotation, targetRotation, 15f * Time.deltaTime);
            
                gameObject.transform.rotation = newRotation;
            }
            
            

            newPosition.y -= 100f * Time.deltaTime;

            _characterController.Move(newPosition * Time.deltaTime);
            
            return;
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
            
            FindVisibleTargets();

            List<CharacterStats> targetsToAttack = new List<CharacterStats>();
            for (int i = 0; i < visibleTargets.Count; i++)
            {
                CharacterStats target = visibleTargets[i].GetComponent<CharacterStats>();
                
                
                if (target != null && ! target.IsDead())
                {
                    targetsToAttack.Add(target);
                }
            }
            actor.combat.Attack(targetsToAttack, viewRadius);

            
            
            
            
//            if (focus == null)
//            {
//                return;
//            }
//
//            if (focus.InInteracableDistance(transform))
//            {
//                focus.Interact();
//            }
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
