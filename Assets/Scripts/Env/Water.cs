using System.Collections.Generic;
using Actors.Base;
using UnityEngine;

namespace Env
{
    public class Water : MonoBehaviour
    {
        private List<Actor> actors = new List<Actor>();
        
        private void OnTriggerEnter(Collider other)
        {
            Actor actor = other.GetComponent<Actor>();

            if (actor != null)
            {
                actors.Add(actor);
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].movement.SetSpeed(0.6f);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Actor actor = other.GetComponent<Actor>();
            
            if (actor != null)
            {
                actors.Remove(actor);
                actor.movement.SetSpeed(1f);
            }
        }
    }
}