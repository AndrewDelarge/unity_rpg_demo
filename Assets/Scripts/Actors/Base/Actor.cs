using UnityEngine;

namespace Actors.Base
{
    [RequireComponent(typeof(Stats))]
    public class Actor : MonoBehaviour
    {
        private GameObject target { get; }
        private Stats actorStats;
        
        private void Awake()
        {
            
        }
        
        
    }
}