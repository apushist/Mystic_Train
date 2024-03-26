using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Transfer : MonoBehaviour
{
	public float newX;
	public float newY;
	public GameObject animationTexture;
	public float animationLength;
	public Light2D playerLight;
	public Light2D globalLight;
	public PlayerController playerController;
	public int newStepSound;
	public float newCameraOrthoSize;
	public BGMController bgmController;
	public bool toDangeon;

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
			float ogPlayerIntensity = playerLight.intensity;
			animator.SetTrigger("");//Todo добавить название триггера
			yield return new WaitForSeconds(animationLength);
			playerLight.intensity = 0;
			playerController.canMove = false;			
			collision.transform.position = new Vector3(newX, newY, 0);
			yield return new WaitForSeconds(0.5f);
			playerController.SetSound(newStepSound);
			playerController.virtualCamera.m_Lens.OrthographicSize = newCameraOrthoSize;
			if (toDangeon)
			{
				bgmController.SetVolume(1, 0f);
				playerLight.intensity = ogPlayerIntensity;
			}
			else
			{
				bgmController.SetVolume(1, 0.17f);
				globalLight.color = new Color(208, 255, 224);
				globalLight.intensity = 0.6f;
			}
			playerController.canMove = true;
		}
	}
}
