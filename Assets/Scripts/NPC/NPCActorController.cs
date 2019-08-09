using System;
using Scriptable;
using UnityEngine;

namespace NPC
{
    public class NPCActorController : MonoBehaviour
    {

        protected NPCActor actor;
        public GameObject lookRadiusObject;
        public GameObject lootBox;

        protected ActorLookRadius actorLookRadius;
        // Start is called before the first frame update
        protected virtual void Start()
        {
            actor = GetComponent<NPCActor>();

            if (actor == null)
            {
                throw new Exception("NPCActor Must Be set in object " + transform.name);
            }

            actorLookRadius = lookRadiusObject.GetComponent<ActorLookRadius>();

            if (actorLookRadius == null)
            {
                throw new Exception("ActorLookRadius Must Be set in Look Radius Object ");
            }

            if (lootBox != null)
            {
                lootBox.SetActive(false);
                actor.characterStats.onDied += ShowLootBox;

            }

            actorLookRadius.onActorEnterRadius += OnActorEnterRadius;
            actorLookRadius.onActorOutRadius += OnActorOutRadius;
            actor.onActorAttacked += Defence;
        }

        protected virtual void OnActorEnterRadius(NPCActor actor)
        {
            //Do smth
        }
        
        protected virtual void OnActorOutRadius(NPCActor actor)
        {
            //Do smth
        }
        

        public virtual void Defence(NPCActor attackedBy)
        {
            if (! actor.InCombat())
            {
                Debug.Log(name + " Start defence!");
                actor.combat.inCombat = true;
                actor.SetTarget(attackedBy);
            }
        }


        void ShowLootBox(GameObject diedObject)
        {
            if (lootBox != null)
            {
                lootBox.SetActive(true);
            }
        }
    }
}
