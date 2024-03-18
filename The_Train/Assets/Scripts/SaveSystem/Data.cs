using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public float[] playerPosition;
    public int[] inventoryItemsIDs;
    public int stepSoundNumber;

    public Data( PlayerController controller,Inventory inventory)
    {
        playerPosition = new float[2];
        playerPosition[0] = controller.transform.position.x;
		playerPosition[1] = controller.transform.position.y;

		List<InventoryItem> items = inventory.GetItems();

		inventoryItemsIDs = new int[items.Count];//нужен метод для получения списка вещей в инвентаре
        for(int i = 0; i < items.Count; i++)
        {
            inventoryItemsIDs[i] = items[i]._id;
        }

        stepSoundNumber = controller.GetSoundNumber();

    }
}
