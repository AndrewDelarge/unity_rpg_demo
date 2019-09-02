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

    protected override void Start()
    {
        base.Start();
        itemNameTextTemplate = Resources.Load<GameObject>("UI/ItemNameText");
    }

    public override void Interact()
    {
        base.Interact();
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

    void OnMouseEnter()
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
            itemNameTextObject.transform.forward = - (UnityEngine.Camera.main.transform.forward);
            itemNameTextObject.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        itemNameTextObject.SetActive(false);
    }

}
