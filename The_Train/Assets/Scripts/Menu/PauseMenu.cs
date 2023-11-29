using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
	public GameObject settingsUI;
    public AudioMixerGroup mixer;

	internal bool isOpened;

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
	{
		isOpened = false;
		pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
	}
    public void Pause()
	{
		isOpened = true;
		pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
	}
    public void QuitGame()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

    public void Settings()
    {
        settingsUI.SetActive(true);
		pauseMenuUI.SetActive(false);
    }

    public void Back()
    {
		settingsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

	public void ChangeMasterVolume(float volume)
	{
		if (volume > 0.5f)
			mixer.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-40, 0, volume));
		else if (volume > 0.25f)
			mixer.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-60, 20, volume));
		else
			mixer.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80, 80, volume));
	}

	public void ChangeMusicVolume(float volume)
	{
		if (volume > 0.5f)
			mixer.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-40, 0, volume));
		else if (volume > 0.25f)
			mixer.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-60, 20, volume));
		else
			mixer.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80, 80, volume));
	}

	public void ChangeEffectsVolume(float volume)
	{
		if (volume > 0.5f)
			mixer.audioMixer.SetFloat("EffectsVolume", Mathf.Lerp(-40, 0, volume));
		else if (volume > 0.25f)
			mixer.audioMixer.SetFloat("EffectsVolume", Mathf.Lerp(-60, 20, volume));
		else
			mixer.audioMixer.SetFloat("EffectsVolume", Mathf.Lerp(-80, 80, volume));
	}
}
