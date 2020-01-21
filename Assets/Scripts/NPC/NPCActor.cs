using Player;
using Scriptable;
using UnityEngine;

namespace NPC
{
    [RequireComponent(typeof(CharacterStats))]
    public class NPCActor : Interactable
    {
        public GameActor actorScript;
        [HideInInspector]
        public CharacterStats characterStats;
        [HideInInspector]
        public NPCActor interactInitedActor;
        [HideInInspector]
        public CharacterCombat combat;
        public NPCActor target;
        public delegate void OnActorAttacked(NPCActor attackedBy);
        public OnActorAttacked onActorAttacked;

        protected virtual void Awake()
        {
            characterStats = GetComponent<CharacterStats>();
            combat = GetComponent<CharacterCombat>();
            Debug.Log(name + " awake");
            Debug.Log(characterStats);
        }

        public override void Interact()
        {
            base.Interact();
            if (interactInitedTransform != null)
            {
                interactInitedActor = interactInitedTransform.GetComponent<NPCActor>();
            }
            Debug.Log("You interact with " + transform.name);
        }

        public virtual void Attack(NPCActor newTarget)
        {
            combat.Attack(newTarget.characterStats, radius);
            if (newTarget.onActorAttacked != null)
            {
                newTarget.onActorAttacked.Invoke(this);
            }
        }
        
        public void SetTarget(NPCActor newTarget)
        {
            if (target == newTarget)
            {
                return;
            }

            target = newTarget;
//            Debug.Log(transform.name + " going attack : " + newTarget.name);
        }

        public void RemoveTarget()
        {
            if (target == null)
            {
                return;
            }
            
//            Debug.Log(transform.name + " remove from target : " + target.name);
            target = null;
        }

        public bool IsEnemy(NPCActor actor)
        {
            return actorScript.fraction.FractionInEnemies(actor.actorScript.fraction);
        }

        public bool IsFriend(NPCActor actor)
        {
            return actorScript.fraction.GetInstanceID() == actor.actorScript.fraction.GetInstanceID();
        }

        public bool InCombat()
        {
            return combat.inCombat;
        }

        public bool IsDead()
        {
            return this.combat.stats.IsDead();
        }
    }
}
