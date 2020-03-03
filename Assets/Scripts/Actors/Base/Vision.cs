using System.Collections.Generic;
using UnityEngine;

namespace Actors.Base
{
    public class Vision : MonoBehaviour
    {
        public bool isEnabled = true;
        public LayerMask defaultLayerMask;
        private float visionUpdateTime = .0f;
        public float viewRadius;
        public float viewAngle = 360f;
        
        public List<GameObject> visibleTargets = new List<GameObject>();
        public List<Actor> actors = new List<Actor>();

        void Start()
        {
            InvokeRepeating(nameof(UpdateVision), 1f, 0.5f);
        }


        void FindVisibleTargets()
        {
            FindVisibleTargets(defaultLayerMask);
        }
        
        void UpdateVision()
        {
            if (isEnabled) {
                FindVisibleTargets();
            }
        }
        
        void FindVisibleTargets(LayerMask mask)
        {
            
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, mask);
            
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                GameObject target = targetsInViewRadius[i].gameObject;
                Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    visibleTargets.Add(target);
                    
//                    float dstToTarget = Vector3.Distance(target.position, transform.position);
//
//                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, interactableMask))
//                    {
//                    }
                    
                }

            }
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
    }
}