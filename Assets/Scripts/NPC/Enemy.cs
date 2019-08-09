using Player;
using UnityEngine;

namespace NPC
{
    public class Enemy : NPCActor
    {
        protected override void Awake()
        {
            base.Awake();

           
        }


        public override void Interact()
        {
            base.Interact();
            if (interactInitedActor != null)
            {
                if (! characterStats.IsDead())
                {
                    interactInitedActor.Attack(this);
                }
                else
                {
                    Loot();
                }
            }
        }
    }
}
