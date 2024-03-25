using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button continueButton;
    private TextMeshProUGUI continueButtonText;

	private void Start()
	{
        continueButtonText = continueButton.GetComponent<TextMeshProUGUI>();
        if (!SaveSystem.SaveFileExists())
        {
            continueButtonText.color = new Color(96, 96, 96);
            continueButton.interactable = false;
        }
	}

	public void NewGame()
    {
        SaveSystem.NewGame();
        SceneLoader.SwitchToScene("Train");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
	{
		PlayerPrefs.SetInt("IsLoading", 1);
	}
}
