using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class Data
{
    public float[] playerPosition;
    public int[] inventoryItemsIDs;
    public int stepSoundNumber;
    public float[] bgmVolumes;
    public float cameraOrthoSize;

	public float[] globalLight;
	public float[] playerLight;

    public int countOfLoreItems;
    public bool threeItemsOnMap;


	public Data(PlayerController controller, Inventory inventory, BGMController bgm, Light2D gLight, Light2D pLight, bool lastThreeItems)
    {
        playerPosition = new float[2];
        playerPosition[0] = controller.transform.position.x;
		playerPosition[1] = controller.transform.position.y;

		

        stepSoundNumber = controller.GetSoundNumber();

        bgmVolumes = bgm.clipVolumes;

        cameraOrthoSize = controller.virtualCamera.m_Lens.OrthographicSize;


        globalLight = new float[5];
        globalLight[0] = gLight.intensity;
        Color gc = gLight.color;
        globalLight[1] = gc.r;
        globalLight[2] = gc.g;
        globalLight[3] = gc.b;
		globalLight[4] = gc.a;



		playerLight = new float[5];
        playerLight[0] = pLight.intensity;
        Color pc = pLight.color;
        playerLight[1] = pc.r;
        playerLight[2] = pc.g;
        playerLight[3] = pc.b;
        playerLight[4] = pc.a;


		List<InventoryItem> items = inventory.GetItems();

		inventoryItemsIDs = new int[items.Count];
		for (int i = 0; i < items.Count; i++)
		{
			inventoryItemsIDs[i] = items[i]._id;
		}

        countOfLoreItems = controller.loreItemsFound;

        threeItemsOnMap = lastThreeItems;
	}
}
