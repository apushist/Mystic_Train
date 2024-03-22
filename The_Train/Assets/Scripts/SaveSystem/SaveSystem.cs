using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
	public static string GetPath()
	{
		return Application.persistentDataPath + "/data.save";
	}

    public static void Save(PlayerController controller, Inventory inventory, BGMController bgmController)
    {
		BinaryFormatter formatter = new BinaryFormatter();
		string path = GetPath();
		FileStream fileStream = new FileStream(path, FileMode.Create);

		Data data = new Data(controller, inventory,bgmController);
		formatter.Serialize(fileStream, data);

		fileStream.Close();
    }

	public static Data Load()
	{
		string path = GetPath();
		if(SaveFileExists())
		{

			BinaryFormatter formatter = new BinaryFormatter();
			FileStream fileStream = new FileStream(path, FileMode.Open);

			Data data = formatter.Deserialize(fileStream) as Data;

			fileStream.Close();
			return data;
		}
		else
		{
			return null;
		}
	}

	public static void NewGame()
	{
		PlayerPrefs.SetInt("IsLoading", 0);
		if(SaveFileExists()) 
			File.Delete(GetPath());

	}

	public static bool SaveFileExists()
	{
		return File.Exists(GetPath());
	}
}
