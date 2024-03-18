using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveSystemObject : MonoBehaviour
{
    public PlayerController controller;
    public Inventory inventory;

	private void Start()
	{
		if(PlayerPrefs.GetInt("IsLoading",0) == 1 && File.Exists(SaveSystem.GetPath()))
		{
			Loading();
		}
	}

	private void ReloadScene()
	{

		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.buildIndex);
	}

	public void SaveFunction()
    {
        SaveSystem.Save(controller, inventory);
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
	}

    public void NewGame()
    {
		PlayerPrefs.SetInt("IsLoading", 0);
		File.Delete(SaveSystem.GetPath());

		ReloadScene();
	}
}
