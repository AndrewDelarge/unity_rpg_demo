using System.Collections;
using System.Collections.Generic;
using Player;
using Scriptable;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    private GameObject itemNameTextTemplate;

    private GameObject itemNameTextObject;
    
    
    private void Start()
    {
        itemNameTextTemplate = Resources.Load<GameObject>("UI/ItemNameText");
        Debug.Log("ItemPickUp");
    }


    public override void Interact()
    {
        PickUp();        
    }


    public void PickUp()
    {
        if (Inventory.instance.Add(item))
        {
            Debug.Log("Item picked: " + item.name);
            Destroy(gameObject);
        }
    }

    private void OnMouseEnter()
    {
        float distance = Vector3.Distance(interactableTransform.position, UnityEngine.Camera.main.transform.position);

        if (itemNameTextObject == null)
        {
            itemNameTextObject = Instantiate<GameObject>(itemNameTextTemplate, transform);
            TextMesh textMesh = (TextMesh) itemNameTextObject.GetComponent(typeof(TextMesh));
            textMesh.text = item.name;
        }
        
        
        if (distance <= 15)
        {
            itemNameTextObject.transform.LookAt(UnityEngine.Camera.main.transform.position);
            itemNameTextObject.SetActive(true);
        }
        
    }


    private void OnMouseExit()
    {
        itemNameTextObject.SetActive(false);
    }

}
