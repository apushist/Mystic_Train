using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.Rendering.Universal;

public class Transfer : MonoBehaviour
{
	public float newX;
	public float newY;
	public GameObject animationTexture;
	public float animationLength;
	public GameObject lightObject;
	public PlayerController playerController;

	private Animator animator;

	private bool safeModeOn;

	private void Start()
	{
		safeModeOn = PlayerPrefs.GetInt("SafeModeOn", 0) == 1;
		animator = animationTexture.GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player")
		{
			StartCoroutine(act(collision));
			
		}
	}

	IEnumerator act(Collider2D collision)
	{
		if (!safeModeOn)
		{
			Light2D light = lightObject.GetComponent<Light2D>();
			animator.SetTrigger("ActivateMonster");//Todo добавить название триггера
			yield return new WaitForSeconds(animationLength);
			light.intensity = 0;
			playerController.canMove = false;			
			collision.transform.position = new Vector3(newX, newY, 0);
			yield return new WaitForSeconds(0.5f);
			light.intensity = 0.9f;
			playerController.canMove = true;
		}
	}
}
