using System.Collections.Generic;
using UnityEngine;

namespace Actors.Base
{
    public class Vision : MonoBehaviour
    {
        public bool isEnabled = true;
        public LayerMask defaultLayerMask;
        public float visionUpdateTime = .5f;
        public float viewRadius;
        public float viewAngle = 360f;
        
        public List<GameObject> visibleTargets = new List<GameObject>();
        public List<Actor> actors = new List<Actor>();

        void Start()
        {
            if (isEnabled)
            {
                InvokeRepeating(nameof(UpdateVision), 1f, visionUpdateTime);
            }
        }

        public void FindVisibleTargets()
        {
            FindVisibleTargets(defaultLayerMask);
        }

        public List<GameObject> FindVisibleColliders()
        {
            return FindVisibleColliders(defaultLayerMask);
        }

        
        void UpdateVision()
        {
            if (isEnabled) {
                FindVisibleTargets();
            }
        }
        
        void FindVisibleTargets(LayerMask mask)
        {
            visibleTargets.Clear();
            actors.Clear();

            List<GameObject> visibleObjects = FindVisibleColliders(mask);
            
            for (int i = 0; i < visibleObjects.Count; i++)
            {
                Actor actor = visibleObjects[i].GetComponent<Actor>();

                if (actor != null && actor.transform != transform)
                {
                    actors.Add(actor);
                }
                    
//                    visibleTargets.Add(target);
                    
//                    float dstToTarget = Vector3.Distance(target.position, transform.position);
//
//                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, interactableMask))
//                    {
//                    }
            }
        }


        public List<GameObject> FindVisibleColliders(LayerMask mask)
        {
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, mask);
            
            List<GameObject> visibleObjects = new List<GameObject>();
            
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                GameObject target = targetsInViewRadius[i].gameObject;
                
                if (IsInViewAngle(target.transform.position) && target.transform != transform)
                {
                    visibleObjects.Add(target);
                }
            }
            
            return visibleObjects;
        }
        
        public Vector3 DirFromAngle(float angleInDegrese, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrese += transform.eulerAngles.y;
            }
            Vector3 result = new Vector3(Mathf.Sin(angleInDegrese * Mathf.Deg2Rad), 0,
                Mathf.Cos(angleInDegrese * Mathf.Deg2Rad));

            return result;
        }

        public bool IsInViewAngle(Vector3 target)
        {
            Vector3 dirToTarget = (target - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                return true;
            }

            return false;
        }
        
    }
}