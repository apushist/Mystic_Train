using UnityEngine;

public enum SoundTriggerType { playOnEnter, stopOnApproach};

public class SoundTrigger : MonoBehaviour
{
	public SoundTriggerType type;
	public GameObject soundTrigger;
	[Header("Stop On Approach")]
	public float distanceToDestroy;


    private AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			audioSource.Play();
			switch (type)
			{
				case SoundTriggerType.playOnEnter: Destroy(soundTrigger, audioSource.clip.length);
					GetComponent<Collider2D>().enabled = false;
					break;
				case SoundTriggerType.stopOnApproach: audioSource.loop = true; break;

			}
			
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && type == SoundTriggerType.stopOnApproach)
		{
			if(Vector2.Distance(collision.transform.position, soundTrigger.transform.position) < distanceToDestroy)
				Destroy(soundTrigger);
			else
				audioSource.Stop();
		}
	}

}
