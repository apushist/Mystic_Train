using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyScript : MonoBehaviour
{
    public bool flag;
    public int i = 1;
}


public class InteractiveZone : MonoBehaviour
{
    [SerializeField] public InteractionType _currentInterType;
    [SerializeField] internal bool _destroyScriptAfterUse = true;
    [SerializeField] internal bool _destroyObjectAfterUse = false;
    [SerializeField] internal GameObject _attachedObjectToDestroy;
    [Header("Puzzle")]
    [SerializeField] public PuzzleBase _puzzle;
    [SerializeField] public InventoryItem _winItem;
    [Header("Lock3Item")]
    [SerializeField] public PuzzleBase _puzzle3;
    [SerializeField] public InventoryItem _winItem3;
    [Header("Door")]
    [SerializeField] public InventoryItem _neededItem;
    [SerializeField] public Door _attachedDoor;


    public bool TrySetItem(InventoryItem item)
    {
        return item._id == _neededItem._id;
    }
    public bool TrySetItem3(InventoryItem item)
    {
        return item._id == _neededItem._id;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            switch (_currentInterType)
            {
                case InteractionType.lockedDoor:
                    Inventory.instance.InteractWithObject(this);
                    break;
                case InteractionType.puzzle:
                    PuzzlesContoller.instance.InteractWithObject(this);
                    break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            switch (_currentInterType)
            {
                case InteractionType.lockedDoor:
                    Inventory.instance.InteractWithObject();
                    break;
                case InteractionType.puzzle:
                    PuzzlesContoller.instance.InteractWithObject();
                    break;
            }
        }
    }
    public bool AfterUse(float delay = 0)
    {
        if (_destroyScriptAfterUse) Destroy(this, delay);
        if (_destroyObjectAfterUse) Destroy(gameObject, delay);
        if(_attachedObjectToDestroy != null) Destroy(_attachedObjectToDestroy, delay);

        if (_destroyObjectAfterUse || _destroyScriptAfterUse) return true;
        else return false;
    }
}

public enum InteractionType { lockedDoor, puzzle, lock3Item }
