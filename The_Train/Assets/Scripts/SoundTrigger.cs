using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
	public GameObject soundTrigger;


    private AudioSource audioSource;

	private void Start()
	{
		audioSource = soundTrigger.GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			audioSource.Play();
			DestroyAfterPlay();
		
		}
	}

	IEnumerator DestroyAfterPlay()
	{
		yield return new WaitForSeconds(audioSource.clip.length);
		DestroyImmediate(audioSource);
	}
}
