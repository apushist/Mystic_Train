using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor.PackageManager.Requests;

public class SaveSystemObject : MonoBehaviour
{
    public PlayerController controller;
    public Inventory inventory;
	public BGMController bgmController;
	public bool isLoading_useWhileEditing;

	private void Start()
	{
		if(isLoading_useWhileEditing && PlayerPrefs.GetInt("IsLoading",0) == 1 && SaveSystem.SaveFileExists())
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
        SaveSystem.Save(controller, inventory,bgmController);
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

		foreach (int id in data.inventoryItemsIDs)
		{
			inventory.AddItem(id);
		}

		bgmController.clipVolumes = data.bgmVolumes;

		controller.virtualCamera.m_Lens.OrthographicSize = data.cameraOrthoSize;
	}

    public void NewGame()
    {
		SaveSystem.NewGame();

		ReloadScene();
	}
}
