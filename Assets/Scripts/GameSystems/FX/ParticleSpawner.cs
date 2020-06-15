using System.Collections;
using UnityEngine;

namespace GameSystems.FX
{
    
    //TODO make it manager
    public class ParticleSpawner : MonoBehaviour
    {
        protected IEnumerator SpawnParticle(GameObject particle, Transform target, float particleLifetime, Quaternion rotation = new Quaternion())
        {
            if (particle == null)
            {
                yield break;
            }
            
            Vector3 pos = target.position;
            pos.y += 1;
            
            GameObject currentParticle = Instantiate(particle, pos, rotation, target);

            
            currentParticle.transform.LookAt(GameController.instance.GetCameraController().GetCamera().transform);
            currentParticle.transform.localScale = transform.localScale;
            
            yield return new WaitForSeconds(particleLifetime);
            
            Destroy(currentParticle);
        }
    }
}