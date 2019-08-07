using System.Collections.Generic;
using Player;
using Scriptable;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    public float radius = 3f;

    private bool isFocus = false;

    protected Transform player;

    protected bool hasInteracted = false;

    public Transform interactableTransform;

    [SerializeField] private List<Item> loot = new List<Item>();

    public System.Action onLootChange;

    public UnityEvent onInteract;
    
    protected virtual void Start()
    {
        if (interactableTransform == null)
        {
            interactableTransform = transform;
        }
    }

    public virtual void Interact()
    {
        if (onInteract != null)
        {
            onInteract.Invoke();
        }
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

    public virtual void Loot()
    {
        Inventory.instance.Loot(this);
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
        
        Inventory.instance.StopLoot();
    }

    public void RemoveFromLoot(Item item)
    {
        loot.Remove(item);
        
        if (onLootChange != null)
        {
            onLootChange();
        }
    }
    
    public void AddToLoot(Item item)
    {
        loot.Add(item);
        
        if (onLootChange != null)
        {
            onLootChange();
        }
    }
    
    public List<Item> GetLoot()
    {
        return loot;
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
