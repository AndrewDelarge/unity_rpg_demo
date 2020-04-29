using Managers.Player;
using Scriptable;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;


    private InventoryManager inventoryManager;
    private GameObject itemNameTextTemplate;
    private GameObject itemNameTextObject;

    protected override void Start()
    {
        base.Start();
        inventoryManager = InventoryManager.instance;
        itemNameTextTemplate = Resources.Load<GameObject>("UI/ItemNameText");
    }

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    public void PickUp()
    {
        if (inventoryManager.Add(item))
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
