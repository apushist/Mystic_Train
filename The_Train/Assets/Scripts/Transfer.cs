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
	private AudioSource audioSource;

	private bool safeModeOn;

	public GameObject overView;
	public GameObject fogParticle;

    private void Start()
	{
		safeModeOn = PlayerPrefs.GetInt("SafeModeOn", 0) == 1;
		animator = animationTexture.GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		overView.SetActive(false);
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
            fogParticle.GetComponent<ParticleSystem>().Play();//start fog particles
            float ogPlayerIntensity = playerLight.intensity;
			audioSource.Play();
			animator.SetTrigger("");//Todo добавить название триггера
            playerController.canMove = false;
            yield return new WaitForSeconds(animationLength);
            overView.SetActive(true);
            playerLight.intensity = 0;			
			collision.transform.position = new Vector3(newX, newY, 0);
			
			playerController.SetSound(newStepSound);
			playerController.virtualCamera.m_Lens.OrthographicSize = newCameraOrthoSize;
            yield return new WaitForSeconds(1f);
            overView.SetActive(false);
            if (toDangeon)
			{
				bgmController.SetVolume(1, 0f);
				playerLight.intensity = ogPlayerIntensity;
			}
			else
			{
				bgmController.SetVolume(1, 0.17f);
				playerController.SetAnimationOnLight();
				playerLight.intensity = 0f;
				globalLight.intensity = 0f;
				globalLight.color = new Color32(158,180,180,255);
				globalLight.intensity = 0.9f;
			}
			Destroy(audioSource); 
			audioSource = null;
			playerController.canMove = true;
			Destroy(fogParticle);//destroy fog effect
		}
	}
}
