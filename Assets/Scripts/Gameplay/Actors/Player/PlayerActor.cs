using System.Collections;
using System.Collections.Generic;
using Gameplay.Actors.Base;
using Gameplay.Actors.Base.Interface;
using GameSystems;
using Managers;
using Managers.Player;
using UnityEngine;

namespace Gameplay.Actors.Player
{
    public class PlayerActor : Actor
    {
        public PlayerFX playerFx;
        private CameraManager cameraManager;
        private EquipmentManager equipment;
        
        
        private bool dashing = false;
        private float dashingColdownd = 1f;
        private float dashingTime;
        private bool isAiming;
        
        public override void Init()
        {
            equipment = PlayerManager.Instance().equipmentManager;
            cameraManager = CameraManager.Instance();

            base.Init();
            
            playerFx.Init(combat);
            // turnoff automatic vision update
            vision.isEnabled = false;
            
            combat.OnAttackEnd += ShakeCamera;
            combat.OnAttackEnd += PushPhysicsObjects;
        }
        
        
        public override void MeleeAttack()
        {
            if (dashing || combat.IsMeleeAttacking())
                return;
            
            combat.MeleeAttack(GetListHealthableInViewRadius());
        }


        public override void Dash()
        {
            if (CanDash())
                StartCoroutine(Dashing());
        }


        public override void Aim(Vector3 point)
        {
            if (equipment.GetRangeWeapon() == null)
                return;

            if (combat.IsRangeCooldown())
                return;
            
            if (combat.aimTime == .0f)
                equipment.SetMainWeaponActive(false);

            isAiming = true;
            float bodyRotationSpeed = 3f;
            
            movement.SetSpeedMultiplier(0.6f);
            combat.Aim();

            point = TransformJoystickPoint(point);
            point.y = animator.lookPoint.position.y;
            
            animator.lookPoint.position = point;
            animator.isLookAtEnabled = true;
            
            // Rotate actor
            if (animator.excessAngle != 0f)
            {
                movement.Rotating(false);
                transform.Rotate(0, animator.excessAngle * Time.deltaTime * bodyRotationSpeed, 0);
            }
        }


        private Vector3 TransformJoystickPoint(Vector3 point)
        {
            Camera camera = cameraManager.GetCamera();
            return camera.transform.TransformPoint(- point) - (camera.transform.position - transform.position);
        }
        
        public override void RangeAttack()
        {
            combat.RangeAttack(animator.lookPoint.position);
        }

        public override void StopAim()
        {
            if (!isAiming)
                return;
            
            if (combat.aimTime > 0)
            {
                RangeAttack();
                movement.ResetSpeedMultiplier();
            }
            
            isAiming = false;
            movement.Rotating(true);
            animator.isLookAtEnabled = false;

            StartCoroutine(ShowMelee());
        }

        IEnumerator ShowMelee(float time = .3f)
        {
            yield return new WaitForSeconds(time);
            equipment.SetMainWeaponActive(true);
        }
        
        public IEnumerator Dashing()
        {
            // todo rework to abilities 
            if (!CanDash())
                yield break;
            
            dashingTime = Time.time;
            dashing = true;
            animator.Trigger("dash");
            movement.SetSpeedMultiplier(3.5f);
            yield return new WaitForSeconds(.15f);
            movement.SetSpeedMultiplier(2);
            yield return new WaitForSeconds(.3f);
            
            movement.SetSpeedMultiplier(1);
            dashing = false;
        }
        
        void PushPhysicsObjects()
        {
            List<Collider> objects = vision.FindVisibleColliders();
            for (int i = 0; i < objects.Count; i++)
            {
                Rigidbody rb = objects[i].attachedRigidbody;

                if (rb != null)
                {
                    if (combat.InMeleeRange(objects[i].transform.position))
                        rb.AddForce(transform.forward * 100, ForceMode.Impulse);
                }
            }
        }

        void ShakeCamera()
        {
            int current = combat.GetCurrentSuccessAttack();
            int multiplier = 1;
            
            if (0 == current)
                multiplier *= 4;

            StartCoroutine(cameraManager.Shake(.25f * multiplier));
        }


        bool CanDash()
        {
            if (Time.time - dashingTime >= dashingColdownd)
            {
                if (combat.IsMeleeAttacking())
                    return false;

                if (combat.aimTime > 0)
                    return false;
                
                return true;
            }
            
            return false;
        }


        public override void PushBack(Vector3 pusherPos, float force = 1)
        {
            StartCoroutine(PushingBack(pusherPos, force));
        }

        IEnumerator PushingBack(Vector3 point, float force = 1)
        {
            float time = 0;
            
            Vector3 pos = transform.position;
            Vector3 endPos = pos - (GetDirection(point) * force);
            
            // Todo check nan
            if (endPos != endPos)
                yield break;
            
            while (time < 1)
            {
                time += Time.deltaTime * 5f;
                transform.position = Vector3.Lerp(pos, endPos, time);
                yield return null;
            }
            
        }

        public List<IHealthable> GetListHealthableInViewRadius()
        {
            List<IHealthable> healthableInViewRadius = new List<IHealthable>();
            
            //Try get more that orig radius
            float oldView = vision.viewAngle;
            vision.viewAngle = 360f;
            vision.viewRadius *= 2f;
            
            List<Collider> objects = vision.FindVisibleColliders();

            vision.viewAngle = oldView;
            vision.viewRadius /= 2f;
            
            for (int i = 0; i < objects.Count; i++)
            {
                IHealthable healthableObject = objects[i].GetComponent<IHealthable>();

                if (healthableObject != null)
                {
                    healthableInViewRadius.Add(healthableObject);
                }
                
            }

            return healthableInViewRadius;
        }

        public override bool IsPlayer()
        {
            return true;
        }
    }
}