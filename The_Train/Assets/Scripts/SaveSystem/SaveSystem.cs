using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void Save(PlayerController controller, Inventory inventory)
    {
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/data.save";
		FileStream fileStream = new FileStream(path, FileMode.Create);

		Data data = new Data(controller, inventory);
		formatter.Serialize(fileStream, data);

		fileStream.Close();
    }

	public static Data Load()
	{
		string path = Application.persistentDataPath + "/data.save";
		if(File.Exists(path))
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
}
