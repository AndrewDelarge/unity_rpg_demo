using UnityEngine;

namespace NPC
{
    public class PlayerActor : NPCActor
    {
        public delegate void OnTargetDied(NPCActor actor);

        public OnTargetDied onTargetDied;
        
        public override void Interact()
        {
            Debug.Log("You cant interact with yourself");
        }

        public void TargetDied(NPCActor actor)
        {
            if (onTargetDied != null)
            {
                onTargetDied.Invoke(actor);
            }
        }
        
    }
}
