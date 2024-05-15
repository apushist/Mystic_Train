using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayItems : MonoBehaviour
{
	[SerializeField] GameObject _supportTextView;
	[SerializeField] GameObject _supportImageView;
	public GameObject attachedDialogue = null;

	private bool nearInteractionObject = false;

	void Start()
	{
		_supportTextView.SetActive(false);
		_supportImageView.SetActive(false);

	}

	void Update()
	{
		if (nearInteractionObject && Input.GetKeyDown(KeyCode.F))
		{
			_supportImageView.SetActive(true);
		}

	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name == "Player")
		{
			nearInteractionObject = true;
			_supportTextView.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.name == "Player")
		{
			nearInteractionObject = false;
			_supportTextView.SetActive(false);
		}
	}

	public void CloseImageAndOpenDialogue()
	{
		_supportImageView.SetActive(false);
		if (attachedDialogue != null)
			attachedDialogue.SetActive(true);
	}
}
