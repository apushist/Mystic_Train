using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemObject : MonoBehaviour
{
    public PlayerController controller;
    public Inventory inventory;

    public void SaveFunction()
    {
        SaveSystem.Save(controller, inventory);
    }

    public void LoadFunction()
    {
        Data data = SaveSystem.Load();

        Vector2 position;
        position.x = data.playerPosition[0];
		position.y = data.playerPosition[1];
        controller.transform.position = position;

        controller.SetSound(data.stepSoundNumber);

        //очистка инвентаря...


        foreach(int id in data.inventoryItemsIDs){
            inventory.AddItem(id);
        }
    }
}
