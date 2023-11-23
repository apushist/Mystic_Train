using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] public int _id;
    [SerializeField] public string _name;
    [SerializeField] public string _description;
    [SerializeField] public Sprite _itemImage;
    [SerializeField] public bool empty = true;
    //[SerializeField] private bool _isStackable = false;
    Image _img;

    public void Init()
    {
        _img = GetComponent<Image>();
        _itemImage = Inventory.instance._itemSpriteEmpty;
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
        _img.sprite = _itemImage;
    }
    public void MouseEnter()
    {
        Inventory.instance.ShowMoreInfoItem(this);
    }
    public void MouseExit()
    {
        Inventory.instance.HideMoreInfoItem(this);
    }
    public void MouseClick()
    {
        Inventory.instance.HideMoreInfoItem(this);
    }

}
