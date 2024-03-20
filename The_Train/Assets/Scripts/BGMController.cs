using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;

    public float[] clipVolumes;

    private float[] nextPlay;

    // Start is called before the first frame update
    void Start()
    {
        nextPlay = new float[clips.Length];
        if (clips.Length != clipVolumes.Length)
            Debug.LogError("BGM: count of clip volumes has to be equal to count of clips");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < clips.Length; i++)
        {
			if (Time.time > nextPlay[i])
			{
                nextPlay[i] = Time.time + clips[i].length;
                audioSource.PlayOneShot(clips[i], clipVolumes[i]);
			}
		}
    }

    public void SetVolume(int soundIndex, float volume)
    {
        clipVolumes[soundIndex] = volume;
    }

}
