using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstLoadScript : MonoBehaviour
{
    public void LoadToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
