using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    public SaveSystemObject saveSystemObject;
	public GameObject objectToDestroy;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		saveSystemObject.SaveFunction();
		Destroy(objectToDestroy);
	}
}
