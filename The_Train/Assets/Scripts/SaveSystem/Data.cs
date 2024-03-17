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

        inventoryItemsIDs = new int[0];//нужен метод для получения списка вещей в инвентаре

        stepSoundNumber = controller.GetSoundNumber();

    }
}
