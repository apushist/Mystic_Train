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
    [SerializeField] public InventoryItem[] _neededItem3;
    [Header("Door")]
    [SerializeField] public InventoryItem _neededItem;
    [SerializeField] public Door _attachedDoor;

    [HideInInspector] public Animator _animator;

    internal bool[] _neededItem3setted;
    internal bool _isLock3Setted = false;
    internal bool _isLockPuzzleSetted = false;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _neededItem3setted = new bool[3];
    }
    public bool TrySetItem(InventoryItem item)
    {
        return item._id == _neededItem._id;
    }
    public bool TrySetItem3(InventoryItem item, int i)
    {
        return item._id == _neededItem3[i]._id;
    }
    public void AddItemNeeded(int i)
    {
        _neededItem3setted[i] = true;
    }
    public bool CheckAllItemNeededSetted()
    {
        for(int i = 0; i <  _neededItem3setted.Length; i++)
        {
            if (_neededItem3setted[i] == false) return false;
        }
        return true;
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
                case InteractionType.lock3Item:
                    if(_isLock3Setted)
                        PuzzlesContoller.instance.InteractWithObject(this);
                    else
                        Inventory.instance.InteractWithObject(this);
                    break;
                case InteractionType.LockedPuzzle:
                    if (_isLockPuzzleSetted)
                        PuzzlesContoller.instance.InteractWithObject(this);
                    else
                        Inventory.instance.InteractWithObject(this);
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
                case InteractionType.lock3Item:
                    Inventory.instance.InteractWithObject();
                    PuzzlesContoller.instance.InteractWithObject();

                    break;
                case InteractionType.LockedPuzzle:
                    Inventory.instance.InteractWithObject();
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

public enum InteractionType { lockedDoor, puzzle, lock3Item, LockedPuzzle }
