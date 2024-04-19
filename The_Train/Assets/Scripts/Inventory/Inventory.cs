using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;



public class Inventory : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] GameObject _inventoryScreen;
    [SerializeField] GameObject _changedItemView;
    [SerializeField] GameObject[] _neededItemView;
    [SerializeField] GameObject _neededItemView3;
    [SerializeField] GameObject _supportTextView;
    [SerializeField] public Sprite _itemSpriteEmpty;
    [SerializeField] InventoryItem _inventoryItemDefault;

    [Header("Item Needed Settings")]
    [SerializeField] Image[] _neededItemSpriteEmpty;
    [SerializeField] Image[] _neededItemSpriteFull;
    [SerializeField] GameObject[] _neededItemOver;

    [Header("Item Scale Settings")]
    [SerializeField] private RectTransform _parentSprite;
    [SerializeField] private float _itemOffset;
    [SerializeField] private float _itemEndOffset;
    [SerializeField] int _itemCount;

    [Header("Item Changed Settings")]
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText;
    [SerializeField] private Image _itemBigImage;
    [SerializeField] GameObject _plank;

    [Header("Item Indicator")]
    [SerializeField] private GameObject _itemIndicator;
    private CanvasGroup _itemIndicatorGroup;

    List<InventoryItem> items = new List<InventoryItem>();
    List<InventoryItem> itemsGrid = new List<InventoryItem>();
    bool canUseInventory = true;
    bool isOpened;
    bool nearInteractionObject;
    internal bool movingItem;
    InteractiveZone currentInteraction;
    float _itemSize;
    public static Inventory instance;

    private AudioSource _audioSource;

    //support fields
    PlayerController pl;
    DialogueManager dm;
    PauseMenu pm;
    InventoryItem moveItem = null;
    InventoryItem startMoveItem = null;
    private void Awake()
    {
        instance = this;
        pl = FindObjectOfType<PlayerController>();
        dm = FindObjectOfType<DialogueManager>();
        pm = FindObjectOfType<PauseMenu>();
        PlayerController.Epressed += PressKeyInventory;
        PlayerController.Fpressed += PressKeyInteraction;
        _audioSource = GetComponent<AudioSource>();
        _itemIndicatorGroup = _itemIndicator.GetComponent<CanvasGroup>();
    }
    private void Start()
    {

        CloseInventory();
        _itemSize = ((_parentSprite.offsetMax.x - _parentSprite.offsetMin.x) - (_itemEndOffset * 2) - (_itemOffset * (_itemCount - 1))) / _itemCount;
        GenerateGrid();
        ChangeInventoryView(0);
        UpdateSupportInteractTextView(false);
        for (int i = 0; i < _neededItemView.Length; i++)
        {
            MouseExitItemNeeded(i);
        }
    }
    public void AddItem(int ident, bool playSound = true)
    {
        InventoryItem item = ItemsData.instance.SearchItemById(ident);
        items.Add(item);
        StopCoroutine(IndicateNewItem(item));
        StartCoroutine(IndicateNewItem(item));
        if (playSound)
            _audioSource.Play();
        Debug.Log(ident);
        for (int i = 0; i < _itemCount * _itemCount; i++)
        {
            if (itemsGrid[i].empty)
            {
                itemsGrid[i].SetNewItem(item, false);
                return;
            }
        }      
        Debug.Log("Full Inventory!");
    }
    private IEnumerator IndicateNewItem(InventoryItem Item)
    {
        _itemIndicator.SetActive(true);
        _itemIndicator.GetComponentInChildren<TextMeshProUGUI>().text = Item._name;
        _itemIndicator.GetComponentInChildren<Image>().sprite = Item._itemImage;
        _itemIndicatorGroup.alpha = 1.0f;
        yield return new WaitForSeconds(2);
        int j = 20;
        for(int i = 0; i < j; i++)
        {
            _itemIndicatorGroup.alpha = 1f - (1f / j * i);
            yield return new WaitForSeconds(1f/j);
        }
        _itemIndicator.SetActive(false);
    }
    public void RemoveItem(int ident)
    {
        bool isIn = false;
        for(var i = 0; i<items.Count; i++)
        {
            if(items[i]._id== ident)
            {
                items.RemoveAt(i);
                isIn = true;
                break;
            }
        }
        if (!isIn)
        {
            Debug.Log("item doesn't exist in inventory");
            return;
        }
        for (int i = 0; i < _itemCount * _itemCount; i++)
        {
            if (itemsGrid[i]._id == ident)
            {
                itemsGrid[i].SetNewItem(_inventoryItemDefault, true);
                break;
            }
        }
        
    }

    public void ShowMoreInfoItem(InventoryItem item)
    {
        _itemNameText.text = item._name;
        _itemDescriptionText.text = item._description;
        _itemBigImage.sprite = item._itemImage;
        _plank.SetActive(true);
    }
    public void HideMoreInfoItem(InventoryItem item)
    {
        _itemNameText.text = "";
        _itemDescriptionText.text = "";
        _itemBigImage.sprite = _itemSpriteEmpty;
        _plank.SetActive(false);
    }

    public void GenerateGrid()
    {
        for (int y = 0; y < _itemCount; y++)
        {
            for (int x = 0; x < _itemCount; x++)
            {
                var spawnedItem = Instantiate(_inventoryItemDefault, _parentSprite);
                spawnedItem.name = $"Tile {x} {y}";
                var rect = spawnedItem.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(x * (_itemSize + _itemOffset) + _itemEndOffset, (_parentSprite.offsetMax.y * 2) - y * (_itemSize + _itemOffset) - _itemEndOffset);
                rect.localScale = new Vector2(_itemSize, _itemSize) / (rect.offsetMax.x - rect.offsetMin.x);
                itemsGrid.Add(spawnedItem);
                spawnedItem.Init();
            }

        }
    }
    public void PressKeyInventory()
    {
        if (!PuzzlesContoller.instance.nearInteractionObject && !dm.isOpened && !pm.isOpened && canUseInventory)
        {
            if (isOpened)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }
    public void PressKeyInteraction()
    {
        if (nearInteractionObject && !PuzzlesContoller.instance.nearInteractionObject && !dm.isOpened && !pm.isOpened && canUseInventory)
        {
            if (isOpened)
            {
                CloseInventory();
            }
            else
            {
                OpenInteractScreen();
            }
        }
    }
    public void OpenInventory()
    {
        pl.canMove = false;
        _inventoryScreen.SetActive(true);
        isOpened = true;
        DeselectAllItems();
        for(int i = 0; i<_neededItemView.Length; i++)
        {
            MouseExitItemNeeded(i);
        }
        ChangeInventoryView(0);
    }
    public void OpenInteractScreen()
    {
        pl.canMove = false;
        _inventoryScreen.SetActive(true);
        isOpened = true;
        DeselectAllItems();
        for (int i = 0; i < _neededItemView.Length; i++)
        {
            MouseExitItemNeeded(i);
        }
        if (currentInteraction._currentInterType == InteractionType.lockedDoor || currentInteraction._currentInterType == InteractionType.LockedPuzzle)
        {
            ChangeInventoryView(1);
            UpdateNeededItemSpriteView(false);
        }
        else if (currentInteraction._currentInterType == InteractionType.lock3Item)
        {
            ChangeInventoryView(2);
            UpdateNeededItemSpriteView3(false, 1);
            UpdateNeededItemSpriteView3(false, 2);
            UpdateNeededItemSpriteView3(false, 3);
        }
    }
    public void CloseInventory()
    {
        if (currentInteraction!=null && currentInteraction._currentInterType == InteractionType.lock3Item && !currentInteraction._isLock3Setted)
        {
            for (int i = 0; i < currentInteraction._neededItem3.Length; i++)
            {
                if (currentInteraction._neededItem3setted[i])
                {
                    AddItem(currentInteraction._neededItem3[i]._id);
                }
                currentInteraction._neededItem3setted[i] = false;
            }
        }
        if (movingItem)
        {
            RevertMovedItem();           
        }
        pl.canMove = true;
        _inventoryScreen.SetActive(false);
        isOpened = false;
    }

    void DeselectAllItems()
    {
        foreach(var i in itemsGrid)
        {
            i.MouseExit();
        }
    }
    void ChangeInventoryView(int b)
    {
        _changedItemView.SetActive(b==0);
        _neededItemView[0].SetActive(b==1);
        _neededItemView3.SetActive(b==2);
    }
    public void InteractWithObject(InteractiveZone col = null)
    {
        if (col != null && col._currentInterType == InteractionType.lock3Item && col._isLock3Setted) return;
        if (col != null)
        {
            currentInteraction = col;
            nearInteractionObject = true;
            UpdateSupportInteractTextView(true);
        }
        else
        {
            currentInteraction = null;
            nearInteractionObject = false;
            UpdateSupportInteractTextView(false);
        }
    }
    public void TrySetItemInteractable()
    {
        if (nearInteractionObject && currentInteraction != null && movingItem)
        {
            bool successed = currentInteraction.TrySetItem(moveItem);
            
            if (successed)
            {
                if (moveItem._itemUseCount > 0)
                {
                    moveItem._itemUseCount--;
                    RevertMovedItem();//multiple use
                }
                else
                {
                    RemoveItem(moveItem._id);
                    DestroyMovedItem();//only one use
                }

                UpdateNeededItemSpriteView(successed);
                if (currentInteraction._currentInterType == InteractionType.LockedPuzzle)
                {                  
                    currentInteraction._isLockPuzzleSetted = true;
                    StartCoroutine(CloseToPuzzle());
                }
                else if(currentInteraction._currentInterType == InteractionType.lockedDoor)
                {
                    currentInteraction._attachedDoor.SetDoorLock(false);
                    StartCoroutine(CloseToGame());
                }              

            }
            else
            {
                RevertMovedItem();
            }
        }
        else
        {
            Debug.Log("error item interact set");
        }
        MouseExitItemNeeded(0);
    }
    public void TrySetItemInteractable3(int i)
    {
        if (nearInteractionObject && currentInteraction != null && movingItem && !currentInteraction.CheckAllItemNeededSetted())
        {
            bool successed = currentInteraction.TrySetItem3(moveItem, i-1);

            if (successed)
            {
                UpdateNeededItemSpriteView3(successed, i);

                DestroyMovedItem();//only one use
                currentInteraction.AddItemNeeded(i-1);
                if (currentInteraction.CheckAllItemNeededSetted())
                {
                    for(int j = 0; j < 3; j++)
                        RemoveItem(currentInteraction._neededItem3[j]._id);
                    currentInteraction._isLock3Setted = true;
                    StartCoroutine(CloseToPuzzle());
                }
            }
            else
            {
                RevertMovedItem();
            }
        }
        else
        {
            Debug.Log("error item interact set");
        }
        MouseExitItemNeeded(0);
    }
    public void MouseEnterItemNeeded(int i)
    {
        if (nearInteractionObject && currentInteraction != null && movingItem)
        {
            _neededItemOver[i].SetActive(true);
        }
    }
    public void MouseExitItemNeeded(int i)
    {
        _neededItemOver[i].SetActive(false);
    }
    void UpdateSupportInteractTextView(bool enabl)
    {
        _supportTextView.SetActive(enabl);
    }
    public void UpdateNeededItemSpriteView(bool succeed)
    {
        _neededItemSpriteEmpty[0].sprite = currentInteraction._neededItem._itemImage;
        _neededItemSpriteFull[0].sprite = currentInteraction._neededItem._itemImage;
        _neededItemSpriteEmpty[0].gameObject.SetActive(!succeed);
        _neededItemSpriteFull[0].gameObject.SetActive(succeed);
    }
    public void UpdateNeededItemSpriteView3(bool succeed, int i)
    {
        _neededItemSpriteEmpty[i].sprite = currentInteraction._neededItem3[i - 1]._itemImage;
        _neededItemSpriteFull[i].sprite = currentInteraction._neededItem3[i - 1]._itemImage;
        _neededItemSpriteEmpty[i].gameObject.SetActive(!succeed);
        _neededItemSpriteFull[i].gameObject.SetActive(succeed);
    }

    public void OnMouseItemClick(InventoryItem item)
    {
        if (movingItem)
        {
            if (item.empty)
            {
                //set
                item.SetNewItem(moveItem, false);
                DestroyMovedItem();
            }
            else
            {
                //swap
                startMoveItem.SetNewItem(item, false);
                item.SetNewItem(moveItem, false);
                DestroyMovedItem();
            }
        }
        else
        {
            if (!item.empty)
            {
                //startMove
                moveItem = CopyItem(item);
                item.SetNewItem(null, true);
                startMoveItem = item;
                movingItem = true;
            }
        }
        item.MouseExit();
        item.MouseEnter();
    }

    InventoryItem CopyItem(InventoryItem item)
    {
        var spawnedItem = Instantiate(_inventoryItemDefault, this.transform);
        spawnedItem.name = "Tile move";
        var rect = spawnedItem.GetComponent<RectTransform>();
        rect.localScale =  new Vector2(_itemSize, _itemSize) / (rect.offsetMax.x - rect.offsetMin.x);
        spawnedItem.Init();
        spawnedItem.SetNewItem(item, false);

        return spawnedItem;
    }
    void RevertMovedItem()
    {
        startMoveItem.SetNewItem(moveItem, false);
        DestroyMovedItem();
    }
    void DestroyMovedItem()
    {
        Destroy(moveItem.gameObject);
        moveItem = null;
        movingItem = false;
    }

    public void BlockInventory()
    {
        canUseInventory = false;
        if(isOpened) CloseInventory();
        UpdateSupportInteractTextView(false);
    }
    private void Update()
    {
        if (movingItem)
        {
            moveItem.GetComponent<RectTransform>().position = Input.mousePosition + new Vector3(10, -10, 0);
        }
    }
    IEnumerator CloseToPuzzle()
    {
        yield return new WaitForSeconds(0.5f);
        CloseInventory();
        var tt = currentInteraction;
        InteractWithObject();//reset last interaction if it destroyed
        PuzzlesContoller.instance.InteractWithObject(tt);
        PuzzlesContoller.instance.StartPuzzleLogic();
        
    }
    IEnumerator CloseToGame()
    {
        yield return new WaitForSeconds(0.5f);
        bool destroyed = currentInteraction.AfterUse();
        if (destroyed)
        {
            InteractWithObject();//reset last interaction if it destroyed
        }
        CloseInventory();
    }
    public List<InventoryItem> GetItems()
    {
        return items;
    }
}
