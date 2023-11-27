using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCollector : MonoBehaviour
{
    [SerializeField] public InventoryItem _neededItem;
    [SerializeField] public Door _attachedDoor;

    public bool TrySetItem(InventoryItem item)
    {
        return item._id == _neededItem._id;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Inventory.instance.InteractWithObject(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Inventory.instance.InteractWithObject();
        }
    }
}
