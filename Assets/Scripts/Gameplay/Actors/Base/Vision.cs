using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Actors.Base
{
    public class Vision : MonoBehaviour
    {
        public bool isEnabled = true;
        public LayerMask defaultLayerMask;
        public float visionUpdateTime = .5f;
        public float viewRadius;
        public float viewAngle = 360f;
        
        public List<Actor> actorsInViewRadius = new List<Actor>();

        void Start()
        {
            if (! isEnabled)
                return;
        
            InvokeRepeating(nameof(UpdateVision), 1f, visionUpdateTime);
        }

        public void FindVisibleTargets()
        {
            FindVisibleTargets(defaultLayerMask);
        }

        public List<Collider> FindVisibleColliders()
        {
            return FindVisibleColliders(defaultLayerMask);
        }
        
        void UpdateVision()
        {
            if (! isEnabled) 
                return;
            
            FindVisibleTargets();
        }
        
        void FindVisibleTargets(LayerMask mask)
        {
            actorsInViewRadius.Clear();

            List<Collider> visibleObjects = FindVisibleColliders(mask);
            
            for (int i = 0; i < visibleObjects.Count; i++)
            {
                Actor actor = visibleObjects[i].GetComponent<Actor>();

                if (actor != null)
                    actorsInViewRadius.Add(actor);

                
//                    visibleTargets.Add(target);
                    
//                    float dstToTarget = Vector3.Distance(target.position, transform.position);
//
//                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, interactableMask))
//                    {
//                    }
            }
        }

        public int GetActorsInViewAngle(out Actor[] actorsInView)
        {
            actorsInView = new Actor[actorsInViewRadius.Count];
            int actorsCount = 0;
            
            for (int i = 0; i < actorsInViewRadius.Count; i++)
            {
                if (IsInViewAngle(actorsInViewRadius[i].transform.position))
                {
                    actorsInView[actorsCount] = actorsInViewRadius[i];
                    actorsCount++;
                }
            }

            return actorsCount;
        }
        
        
        public List<Collider> FindVisibleColliders(LayerMask mask)
        {
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, mask);
            
            List<Collider> visibleObjects = new List<Collider>();
            
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                GameObject target = targetsInViewRadius[i].gameObject;
                
                if (target.transform != transform)
                    visibleObjects.Add(targetsInViewRadius[i]);
            }
            
            return visibleObjects;
        }
        
        public Vector3 DirFromAngle(float angleInDegrese, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
                angleInDegrese += transform.eulerAngles.y;

            Vector3 result = new Vector3(Mathf.Sin(angleInDegrese * Mathf.Deg2Rad), 0,
                Mathf.Cos(angleInDegrese * Mathf.Deg2Rad));

            return result;
        }

        public bool IsInViewAngle(Vector3 target)
        {
            Vector3 dirToTarget = (target - transform.position).normalized;
            
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                return true;

            return false;
        }
        
        public bool IsInViewAngle(Transform target)
        {
            return IsInViewAngle(target.position);
        }
    }
}