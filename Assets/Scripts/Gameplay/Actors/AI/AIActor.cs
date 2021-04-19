using System.Collections;
using System.Collections.Generic;
using Gameplay.Actors.Base;
using Gameplay.Actors.Base.Interface;
using Gameplay.Actors.Base.StatsStuff;
using UI;
using UnityEngine;

namespace Gameplay.Actors.AI
{
    [RequireComponent(typeof(AICombat))]
    public class AIActor : Actor
    {
        public Collider actorColider{ get; protected set; }
        private Collider[] skilletColliders;

        private AIActorFX actorFx;
        private AIDialog dialog;

        public float ragdollForceMultiplier = 15f;
        
        public override void Init()
        {
            base.Init();

            InitComponents();
            
            SetSkilletColliderActivity(false);
//            stats.onGetDamage += PushBack;
        }

        private void InitComponents()
        {
            actorFx = GetComponent<AIActorFX>();
            if (actorFx != null)
                actorFx.Init();

            dialog = GetComponent<AIDialog>();
            if (dialog != null)
                dialog.Init();

            HealthUI healthUi = GetComponent<HealthUI>();
            if (healthUi != null)
                healthUi.Init();
            
            actorColider = GetComponent<Collider>();
            
            skilletColliders = GetComponentsInChildren<Collider>();
        }


        void PushBack(Damage damage)
        {
            float power = .1f;
            
            if (damage.IsCrit())
                power *= .5f;

            Actor owner = damage.GetOwner();
            Vector3 pos = transform.forward;


            if (owner != null)
                pos = transform.InverseTransformPoint(owner.transform.position);
            
            transform.position = transform.TransformPoint((-pos + Vector3.up) * power);
        }
        
        protected override void Die(GameObject go)
        {
            base.Die(go);
            actorColider.enabled = false;
            Ragdoll();
        }

        void Ragdoll()
        {
            SetSkilletColliderActivity(true);
            
            Vector3 playerDirection = GetDirection(GetLastDamageDealerPosition());
            playerDirection.y = -0.2f;
            
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                
                rigidbody.AddForce(- playerDirection * ragdollForceMultiplier, ForceMode.Impulse);
            }
        }

        void SetSkilletColliderActivity(bool activity)
        {
            foreach (Collider collider in skilletColliders)
            {
                if (collider == actorColider)
                    continue;
                
                collider.isTrigger = ! activity;
            }
        }
        
        public override void MeleeAttack(IHealthable target)
        {
            if (combat.InMeleeRange(target.GetTransform()))
            {
                movement.FaceTarget(target.GetTransform().position);

                var attackList = new List<IHealthable> {target};

                combat.MeleeAttack(attackList);
            }
        }


        public override void PushBack(Vector3 pusherPos, float force = 1)
        {
            StartCoroutine(PushingBack(pusherPos, force));
        }

        void ToggleAi(bool active)
        {
            input.enabled = active;
        }

        IEnumerator PushingBack(Vector3 point, float force = 1)
        {
//            ToggleAi(false);
            float time = 0;
            
            Vector3 pos = transform.position;
            Vector3 endPos = pos - (GetDirection(point) * force);
            
            while (time < 1)
            {
                time += Time.deltaTime * 10f;
                transform.position = Vector3.Lerp(pos, endPos, time);
                yield return null;
            }
            
//            ToggleAi(true);
        }
    }
}