using System.Collections.Generic;
using UnityEngine;

public class ItemsData : MonoBehaviour
{
    [SerializeField] public List<InventoryItem> _itemsData;
    public static ItemsData instance;
    private void Awake()
    {
        instance = this;
    }

    public InventoryItem SearchItemById(int id)
    {
        foreach(var i in _itemsData)
        {
            if(i._id == id)
            {
                return i;
            }
        }
        Debug.Log("item not finded");
        return null;
    }
}
