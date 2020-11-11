using Gameplay.Actors.Base;
using Gameplay.Actors.Base.Interface;
using GameSystems;
using GameSystems.FX;
using Managers;
using UI;
using UnityEngine;

namespace Gameplay.Actors.AI
{
    public class AIActorFX : ParticleSpawner
    {

        public GameObject hitParticle;
        public GameObject healParticle;
        
        public float particleLifetime;
        
        
        private IHealthable stats;
        private Transform target;
        private WorldUiCanvas worldUiCanvas;
        public void Init()
        {
            worldUiCanvas = GameController.instance.sceneController.worldUiCanvas;
            stats = GetComponent<IHealthable>();
            stats.OnHealthChange += ShowHealChange;
            stats.OnHealthChange += ShowDamageText;
            target = transform;
            Transform targetRend = GetComponentInChildren<Transform>();
            if (targetRend != null)
            {
                target = targetRend.transform;
            }
            
        }

        void ShowHealChange(object healthable, HealthChangeEventArgs args)
        {
            if (args.healthChange > 0)
            {
                StartCoroutine(SpawnParticle(healParticle, target, particleLifetime));
            }
            else if (args.healthChange < 0)
            {
                if (args.initiator != null)
                {
                    Quaternion quaternion = new Quaternion();
                    quaternion.SetLookRotation(args.initiator.transform.position);
                    StartCoroutine(SpawnParticle(hitParticle, target, particleLifetime, quaternion));
                    return;
                }
                StartCoroutine(SpawnParticle(hitParticle, target, particleLifetime));
            }

        }

        void ShowDamageText(object healthable, HealthChangeEventArgs args)
        {
            if (worldUiCanvas.worldUiObjects.damageTextFeed == null)
            {
                return;
            }

            Actor owner = args.modifier.GetOwner();
            
            // TODO hadcoded tag
            // Only player damage showing
            if (owner == null || ! owner.CompareTag("Player"))
            {
                return;
            }
            GameController.instance.uiManager.AddDamageFeed(transform, args.modifier.GetValue().ToString(), args.modifier.IsCrit());
        }
    }
    
    
}