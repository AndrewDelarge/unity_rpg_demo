
using System.Collections;
using System.Collections.Generic;
using Actors.Base;
using Actors.Base.Interface;
using GameSystems;
using Managers.Player;
using Scriptable;
using UnityEngine;

namespace Actors.Player
{
    public class PlayerActor : Actor
    {
        public PlayerFX playerFx;
        private CameraController cameraController;
        private EquipmentManager equipment;
        
        
        private bool dashing = false;
        private float dashingColdownd = 1f;
        private float dashingTime;
        private float aimTime;
        
        public override void Init()
        {
            base.Init();
            playerFx = GetComponent<PlayerFX>();
            playerFx.Init(combat);
            // turnoff automatic vision update
            vision.isEnabled = false;
            cameraController = GameController.instance.cameraController;
            equipment = GameController.instance.playerManager.equipmentManager;
            combat.OnAttackEnd += ShakeCamera;
            combat.OnAttackEnd += PushPhysicsObjects;
        }
        
        
        public override void MeleeAttack()
        {
            if (dashing)
            {
                return;
            }
            
            float oldView = vision.viewAngle;
            vision.viewAngle = 360f;
            vision.viewRadius *= 2f;
            List<Collider> objects = vision.FindVisibleColliders();

            vision.viewAngle = oldView;
            vision.viewRadius /= 2f;
            List<IHealthable> attack = new List<IHealthable>();
            
            for (int i = 0; i < objects.Count; i++)
            {
                IHealthable healthableObject = objects[i].GetComponent<IHealthable>();

                if (healthableObject != null)
                {
                    attack.Add(healthableObject);
                }
                
            }
            combat.MeleeAttack(attack);
        }


        public override void Dash()
        {
            if (CanDash())
            {
                StartCoroutine(Dashing());
            }
        }


        public override void Aim(Vector3 point)
        {
            if (combat.aimTime == .0f)
            {
                equipment.SetVisibleMelee(false);
                equipment.SetVisibleRange(true);
            }
            combat.Aim();

            Transform cameraPos = GameController.instance.cameraController.GetCamera().transform;
            point = - point;
            animator.isLookAtEnabled = true;

            point = cameraPos.TransformPoint(point) - (cameraPos.position - transform.position);
            point.y = animator.lookPoint.position.y;
            animator.lookPoint.position = point;

            if (animator.excessAngle != 0)
            {
                movement.Rotating(false);
                transform.Rotate(0, animator.excessAngle * Time.deltaTime * 3f, 0);
            }
            
            Debug.DrawLine(transform.position, point);
        }

        public override void RangeAttack()
        {
            combat.RangeAttack(animator.lookPoint.position);
        }

        public override void StopAim()
        {
            if (combat.aimTime > 0)
            {
                RangeAttack();
            }
            movement.Rotating(true);
            animator.isLookAtEnabled = false;
            
            equipment.SetVisibleMelee(true);
            equipment.SetVisibleRange(false);
            // show main weapon hide primary weapon
        }

        public IEnumerator Dashing()
        {
            // TODO remove hardcode dashing?
            if (!CanDash())
            {
                yield break;
            }
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
            int multyplier = 2;
            if (0 == current)
            {
                multyplier *= 2;
            }

            StartCoroutine(cameraController.Shake(.1f * multyplier, 1f / 2));
        }


        bool CanDash()
        {
            if (Time.time - dashingTime >= dashingColdownd)
            {
                if (combat.IsAttacking())
                {
                    return false;
                }

                if (aimTime > 0)
                {
                    return false;
                }
                
                return true;
            }
            
            return false;
        }
    }
}