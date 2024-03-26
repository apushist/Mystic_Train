using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainSettings : MonoBehaviour
{
    public GameObject settingsUI;
    public AudioMixerGroup mixer;
    public TMP_Dropdown resolution;

    private void Start()
    {

        foreach (Slider slider in settingsUI.GetComponentsInChildren<Slider>())
        {
            slider.value = PlayerPrefs.GetFloat(slider.name, 1);
            PlayerPrefs.SetFloat(slider.name, slider.value);
        }
        resolution.value = PlayerPrefs.GetInt("resolution", 4);
        PlayerPrefs.SetInt("resolution", resolution.value);

        Close();
    }

    public void Close()
    {
        settingsUI.SetActive(false);
    }
    public void Open()
    {
        settingsUI.SetActive(true);
    }
    public void ChangeResolution()
    {
        if(resolution.value == 0)
        {
            Screen.SetResolution(1280,720, true);
        }
        else if(resolution.value == 1)
        {
            Screen.SetResolution(1280,1024, true);
        }
        else if (resolution.value == 2)
        {
            Screen.SetResolution(1366,768, true);
        }
        else if (resolution.value == 3)
        {
            Screen.SetResolution(1280, 720,true);
        }
        else if (resolution.value == 4)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        else if (resolution.value == 5)
        {
            Screen.SetResolution(2560,1440, true);
        }
        else if (resolution.value == 6)
        {
            Screen.SetResolution(3840,2160, true);
        }
        else if (resolution.value == 7)
        {
            Screen.SetResolution(144, 81, true);
        }
        PlayerPrefs.SetInt("resolution", resolution.value);
    }
    public void ChangeMasterVolume(float volume)
    {
        ChangeVolume(volume, "MasterVolume");
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void ChangeMusicVolume(float volume)
    {
        ChangeVolume(volume, "MusicVolume");
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void ChangeEffectsVolume(float volume)
    {
        ChangeVolume(volume, "EffectsVolume");
        PlayerPrefs.SetFloat("EffectsVolume", volume);
    }

    void ChangeVolume(float volume, string name)
    {
        
        if (volume > 0.5f)
            mixer.audioMixer.SetFloat(name, Mathf.Lerp(-40, 0, volume));
        else if (volume > 0.25f)
            mixer.audioMixer.SetFloat(name, Mathf.Lerp(-60, 20, volume));
        else
            mixer.audioMixer.SetFloat(name, Mathf.Lerp(-80, 80, volume));
    }
}
