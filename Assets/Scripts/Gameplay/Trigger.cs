using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class Trigger : MonoBehaviour
    {
        [TagSelector]
        public string activeTag;
        public bool disableAfterTrigger = false;
        [SerializeField]
        public UnityEvent OnEnter;
        [SerializeField]
        public UnityEvent OnExit;

        public void Init()
        {
            gameObject.SetActive(true);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (! IsTagAllowed(other.gameObject))
            {
                return;
            }

            OnEnter?.Invoke();
            
            if (disableAfterTrigger)
            {
                gameObject.SetActive(false);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (! IsTagAllowed(other.gameObject))
            {
                return;
            }

            OnExit?.Invoke();
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
