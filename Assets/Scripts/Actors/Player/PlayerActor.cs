
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
        private CameraController cameraController;
        
//        protected override void Awake()
//        {
//            return;
//        }

        public override void Init()
        {
            base.Init();
            // turnoff automatic vision update
            vision.isEnabled = false;
            cameraController = Camera.main.GetComponent<CameraController>();
            combat.OnAttackEnd += () => StartCoroutine(cameraController.Shake(.1f, 1f));
        }
        public override void MeleeAttack()
        {
            float oldView = vision.viewAngle;
            vision.viewAngle = 360f;
            vision.viewRadius *= 2f;
            List<GameObject> objects = vision.FindVisibleColliders();
            
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

    }
}