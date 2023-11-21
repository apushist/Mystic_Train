using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public GameObject doorPrefab;
	public bool isLocked;
	private Animator anim;

	// Start is called before the first frame update
	void Start()
    {
        anim = doorPrefab.GetComponent<Animator>();
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isLocked)
		{
			anim.Play("OpenDoor");
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!isLocked)
		{
			anim.Play("Opened");
		}
	}


	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!isLocked)
		{
			anim.Play("CloseDoor");
		}
	}
}
