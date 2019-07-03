using UnityEngine;

namespace NPC
{
    public class PlayerActor : NPCActor
    {
        
        public override void Interact()
        {
            Debug.Log("You cant interact with yourself");
        }
    }
}
