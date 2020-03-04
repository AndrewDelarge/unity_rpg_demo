using System.Collections.Generic;
using Actors.Base;
using GameInput;
using UnityEngine;

namespace Actors.AI
{
    public class AICombat : Base.Combat
    {
        protected override void InputAttack()
        {
            if (targetActor == null)
            {
                return;
            }
            
            List<Stats> attackList = new List<Stats>();
            attackList.Add(targetActor.stats);
            
            MeleeAttack(attackList);
        }

        
    }
}