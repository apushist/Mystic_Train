using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SaveSystemObject : MonoBehaviour
{
	public Inventory inventory;
	public PlayerController controller;
	public BGMController bgmController;
	public Light2D globalLight;
	public Light2D playerLight;
	public bool isLoading_useWhileEditing;


	private void Start()
	{
		if(isLoading_useWhileEditing && PlayerPrefs.GetInt("IsLoading",0) == 1)
		{
			Loading();
		}
	}

	private void ReloadScene()
	{
		SceneLoader.SwitchToScene("Train");
	}

	public void SaveFunction()
    {
		
        SaveSystem.Save(controller, inventory,bgmController,globalLight,playerLight);
    }

    public void LoadFunction()
	{
		PlayerPrefs.SetInt("IsLoading", 1);
		ReloadScene();
    }

	private void Loading()
	{
		Data data = SaveSystem.Load();

		Vector2 position;
		position.x = data.playerPosition[0];
		position.y = data.playerPosition[1];
		controller.transform.position = position;

		controller.SetSound(data.stepSoundNumber);

		

		bgmController.clipVolumes = data.bgmVolumes;

		controller.virtualCamera.m_Lens.OrthographicSize = data.cameraOrthoSize;

		float[] gl = data.globalLight;
		globalLight.intensity = gl[0];
		globalLight.color = new Color(gl[1], gl[2], gl[3], gl[4]);

		float[] pl = data.playerLight;
		playerLight.intensity = pl[0];
		playerLight.color = new Color(pl[1], pl[2], pl[3], pl[4]);

		foreach (int id in data.inventoryItemsIDs)
		{
			inventory.AddItem(id, false);
		}
	}

    public void NewGame()
    {
		SaveSystem.NewGame();

		ReloadScene();
	}
}
