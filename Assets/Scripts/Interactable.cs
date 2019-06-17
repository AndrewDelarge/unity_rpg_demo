using UnityEngine;

public class Interactable : MonoBehaviour
{

    public float radius = 3f;

    private bool isFocus = false;

    protected Transform player;

    protected bool hasInteracted = false;

    public Transform interactableTransform;

    private void Start()
    {
        Debug.Log("Iteracable start");
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
            float distance = Vector3.Distance(interactableTransform.position, player.position);
            if (distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
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
