using Player;
using UnityEngine;

namespace NPC
{
    public class EnemyStats : CharacterStats
    {
        public override void Die()
        {
            base.Die();
            
//            Destroy(gameObject);
        }
        
      
    }
}
