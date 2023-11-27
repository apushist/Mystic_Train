using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public GameObject doorManager;
	public bool isLocked;
	private Animator anim;

	// Start is called before the first frame update
	void Start()
    {
        anim = doorManager.GetComponent<Animator>();
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isLocked)
		{
			anim.SetBool("IsPlayerNear", true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!isLocked)
		{
			anim.SetBool("IsPlayerNear", false);
		}
	}
	public void SetDoorLock(bool isOn)
    {
		isLocked = isOn;
		GetComponent<Collider2D>().enabled = false;
		GetComponent<Collider2D>().enabled = true;
	}
}
