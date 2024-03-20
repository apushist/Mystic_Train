using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] GameObject _inventoryScreen;
    [SerializeField] GameObject _changedItemView;
    [SerializeField] GameObject _neededItemView;
    [SerializeField] GameObject _supportTextView;
    [SerializeField] public Sprite _itemSpriteEmpty;
    [SerializeField] InventoryItem _inventoryItemDefault;

    [Header("Item Needed Settings")]
    [SerializeField] Image _neededItemSpriteEmpty;
    [SerializeField] Image _neededItemSpriteFull;
    [SerializeField] GameObject _neededItemOver;
    //[SerializeField] private TextMeshProUGUI _itemNeededCounter;

    [Header("Item Scale Settings")]
    [SerializeField] private RectTransform _parentSprite;
    [SerializeField] private float _itemOffset;
    [SerializeField] private float _itemEndOffset;
    [SerializeField] int _itemCount;

    [Header("Item Changed Settings")]
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText;
    [SerializeField] private Image _itemBigImage;

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
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {

        CloseInventory();
        _itemSize = ((_parentSprite.offsetMax.x - _parentSprite.offsetMin.x) - (_itemEndOffset * 2) - (_itemOffset * (_itemCount - 1))) / _itemCount;
        GenerateGrid();
        ChangeInventoryView(true);
        UpdateSupportInteractTextView(false);
        MouseExitItemNeeded();
    }
    public void AddItem(int ident)
    {
        InventoryItem item = ItemsData.instance.SearchItemById(ident);
        items.Add(item);
        _audioSource.Play();
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

    public void ShowMoreInfoItem(InventoryItem item)
    {
        _itemNameText.text = item._name;
        _itemDescriptionText.text = item._description;
        _itemBigImage.sprite = item._itemImage;
    }
    public void HideMoreInfoItem(InventoryItem item)
    {
        _itemNameText.text = "";
        _itemDescriptionText.text = "";
        _itemBigImage.sprite = _itemSpriteEmpty;
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
    public void OpenInventory()
    {
        pl.canMove = false;
        _inventoryScreen.SetActive(true);
        isOpened = true;
        DeselectAllItems();
        MouseExitItemNeeded();
        if (nearInteractionObject)
        {
            ChangeInventoryView(false);
            UpdateNeededItemSpriteView(false);
        }
        else
        {
            ChangeInventoryView(true);
        }
    }
    public void CloseInventory()
    {
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
    void ChangeInventoryView(bool b)
    {
        _changedItemView.SetActive(b);
        _neededItemView.SetActive(!b);
    }
    public void InteractWithObject(InteractiveZone col = null)
    {
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
                UpdateNeededItemSpriteView(successed);
                currentInteraction._attachedDoor.SetDoorLock(false);
                if (moveItem._itemUseCount > 0)
                {
                    moveItem._itemUseCount--;
                    RevertMovedItem();//multiple use
                }
                else
                    DestroyMovedItem();//only one use

                bool destroyed = currentInteraction.AfterUse();
                if (destroyed)
                {
                    InteractWithObject();//reset last interaction if it destroyed
                }
                CloseInventory();
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
        MouseExitItemNeeded();
    }
    public void MouseEnterItemNeeded()
    {
        if (nearInteractionObject && currentInteraction != null && movingItem)
        {
            _neededItemOver.SetActive(true);
        }
    }
    public void MouseExitItemNeeded()
    {
        _neededItemOver.SetActive(false);
    }
    void UpdateSupportInteractTextView(bool enabl)
    {
        _supportTextView.SetActive(enabl);
    }
    public void UpdateNeededItemSpriteView(bool succeed)
    {
        _neededItemSpriteEmpty.sprite = currentInteraction._neededItem._itemImage;
        _neededItemSpriteFull.sprite = currentInteraction._neededItem._itemImage;
        _neededItemSpriteEmpty.gameObject.SetActive(!succeed);
        _neededItemSpriteFull.gameObject.SetActive(succeed);
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

    public List<InventoryItem> GetItems()
    {
        return items;
    }
}
