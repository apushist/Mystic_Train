using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [Header("ItemSettings")]
    [SerializeField] public int _id;
    [SerializeField] public string _name;
    [SerializeField] public string _description;
    [SerializeField] public Sprite _itemImage;
    [Header("ObjectSettings")]
    [SerializeField] public bool empty = true;
    //[SerializeField] private bool _isStackable = false;
    [SerializeField] Image _objectImage;
    [SerializeField] GameObject _overlayImage;

    public void Init()
    {
        _itemImage = Inventory.instance._itemSpriteEmpty;
        _overlayImage.SetActive(false);
    }
    public void SetNewItem(InventoryItem item, bool isEmpty)
    {
        if (!isEmpty) {
            _id = item._id;
            _name = item._name;
            _itemImage = item._itemImage;
            _description = item._description;
            empty = false;
        }
        else
        {
            _id = 0;
            _name = "";
            _itemImage = Inventory.instance._itemSpriteEmpty;
            _description = "";
            empty = true;
        }
        UpdateImage();
    }
    public void UpdateImage()
    {
        _objectImage.sprite = _itemImage;
    }
    public void MouseEnter()
    {
        if (!empty && !Inventory.instance.movingItem) {
            _overlayImage.SetActive(true);
            Inventory.instance.ShowMoreInfoItem(this);
        }
        if (Inventory.instance.movingItem)
        {
            _overlayImage.SetActive(true);
        }
    }
    public void MouseExit()
    {
        _overlayImage.SetActive(false);
        Inventory.instance.HideMoreInfoItem(this);
    }
    public void MouseClick()
    {
        Inventory.instance.OnMouseItemClick(this);
    }

}
