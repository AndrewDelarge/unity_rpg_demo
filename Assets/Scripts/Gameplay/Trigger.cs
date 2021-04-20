using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class Trigger : MonoBehaviour
    {
        [TagSelector]
        public string activeTag;
        
        public bool disableOnEnterTrigger = false;
        public bool disableOnExitTrigger = false;
        
        [SerializeField] public UnityEvent OnEnter;
        [SerializeField] public UnityEvent OnExit;

        public void Init()
        {
            gameObject.SetActive(true);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (! IsTagAllowed(other.gameObject))
                return;

            OnEnter?.Invoke();
            
            if (disableOnEnterTrigger)
                gameObject.SetActive(false);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (! IsTagAllowed(other.gameObject))
                return;

            OnExit?.Invoke();
            
            if (disableOnExitTrigger)
                gameObject.SetActive(false);
        }

        bool IsTagAllowed(GameObject gameObject)
        {
            if (activeTag == "")
                return true;
            
            return gameObject.tag == activeTag;
        }
    }

    public class TagSelectorAttribute : PropertyAttribute
    {
    }
}
