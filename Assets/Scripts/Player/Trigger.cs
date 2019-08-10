using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;


namespace Player
{
    public class Trigger : MonoBehaviour
    {
        [TagSelector]
        public string activeTag;
        
        [SerializeField]
        private UnityEvent OnEnter;
        [SerializeField]
        private UnityEvent OnExit;

        private void OnTriggerEnter(Collider other)
        {
            if (! IsTagAllowed(other.gameObject))
            {
                return;
            }

            if (OnEnter != null)
            {
                OnEnter.Invoke();
            }
        }
        
        
        private void OnTriggerExit(Collider other)
        {
            if (! IsTagAllowed(other.gameObject))
            {
                return;
            }

            if (OnExit != null)
            {
                OnExit.Invoke();
            }
        }


        bool IsTagAllowed(GameObject gameObject)
        {
            if (activeTag == "")
            {
                return true;
            }
            
            if (gameObject.tag == activeTag)
            {
                return true;
            }

            return false;
        }
    }

    public class TagSelectorAttribute : PropertyAttribute
    {
    }
}
