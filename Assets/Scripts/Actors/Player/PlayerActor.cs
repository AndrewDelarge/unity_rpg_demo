
using System.Collections.Generic;
using Actors.Base;
using Actors.Base.Interface;
using GameCamera;
using Managers.Player;
using Scriptable;
using UnityEngine;

namespace Actors.Player
{
    public class PlayerActor : Actor
    {
        public PlayerFX playerFx;
        private CameraController cameraController;
        
        public override void Init()
        {
            base.Init();
            playerFx = GetComponent<PlayerFX>();
            playerFx.Init(combat);
            // turnoff automatic vision update
            vision.isEnabled = false;
            cameraController = Camera.main.GetComponent<CameraController>();
            combat.OnAttackEnd += ShakeCamera;
            combat.OnAttackEnd += PushPhysicsObjects;
        }
        public override void MeleeAttack()
        {
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
    }
}