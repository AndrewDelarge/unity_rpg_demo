using UnityEngine;

public class Interactable : MonoBehaviour
{

    public float radius = 3f;

    private bool isFocus = false;

    protected Transform player;

    protected bool hasInteracted = false;

    public Transform interactableTransform;
    

    protected virtual void Start()
    {
        if (interactableTransform == null)
        {
            interactableTransform = transform;
        }
    }

    public virtual void Interact()
    {
        
    }
    
    private void Update()
    {
        if (isFocus && ! hasInteracted)
        {
            if (InInteracableDistance(player))
            {
//                Interact();
                hasInteracted = true;
            }
        }
    }


    public bool InInteracableDistance(Transform target)
    {
        float distance = Vector3.Distance(interactableTransform.position, target.position);
        return distance <= radius;
    }


    public void OnFocused(Transform transformPlayer)
    {
        isFocus = true;
        player = transformPlayer;
        hasInteracted = false;
    }

    public void OnDisfocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }
    
    
    private void OnDrawGizmosSelected()
    {
        if (interactableTransform == null)
        {
            interactableTransform = transform;
        }
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(interactableTransform.position, radius);   
    }
}
