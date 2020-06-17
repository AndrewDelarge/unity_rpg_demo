using System.Collections;
using Actors.Base;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Zones
{
    public class PushBackZone : MonoBehaviour
    {
        public float radius = 2f;
        public int iterations = 1;
        public float startDelay = 0f;
        public float iterDelay = 0.5f;
        public float force = 1f;
        
        
        public bool startOnSpawn = false;
        private void Start()
        {
            if (startOnSpawn)
            {
                StartPushing();
            }
        }

        public void StartPushing()
        {
            StartCoroutine(Pushing());
        }

        public static void PushBackActors(Vector3 center, float raduis, float force = 1f)
        {
            RaycastHit[] raycastHit = Physics.SphereCastAll(center, raduis, Vector3.forward, raduis);
            
            for (int i = 0; i < raycastHit.Length; i++)
            {
                Actor actor = raycastHit[i].collider.gameObject.GetComponent<Actor>();
                if (actor != null)
                {
                    actor.PushBack(center, force);
                }
            }
        }

        IEnumerator Pushing()
        {
            yield return new WaitForSeconds(startDelay);

            for (int i = 0; i < iterations; i++)
            {
                PushBackActors(transform.position, radius, force);
                yield return new WaitForSeconds(iterDelay);
            }
        }
    }
}