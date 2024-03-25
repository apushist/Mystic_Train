using System.Collections;
using UnityEngine;
using TMPro;

public class FPScounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textField;
    [SerializeField] lockStates _lockState;
    int fps = 0;
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        switch (_lockState)
        {
            case lockStates.lock30:              
                Application.targetFrameRate = 30;
                break;
            case lockStates.lock60:
                Application.targetFrameRate = 60;
                break;
            case lockStates.lock120:
                Application.targetFrameRate = 120;
                break;
            case lockStates.unlock:
                Application.targetFrameRate = -1;
                break;
        }
        StartCoroutine(updateFrame(0.2f));
        
    }

    private IEnumerator updateFrame(float t)
    {
        while (true)
        {
            fps = (int)(1f / Time.unscaledDeltaTime);
            _textField.text = fps.ToString();
            yield return new WaitForSeconds(t);
        }
    }

    enum lockStates
    {
        lock30, lock60, lock120, unlock
    }
}
