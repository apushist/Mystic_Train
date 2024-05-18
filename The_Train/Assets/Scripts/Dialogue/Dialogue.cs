using UnityEngine;

[System.Serializable]
public class Dialogue
{

	public bool partOfLore = false;

	public string name;

	[TextArea(3,10)]
	public string[] sentences;

}
