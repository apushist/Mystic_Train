using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FPScounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textField;
    [SerializeField] lockStates _lockState;
    int fps = 0;
    void Awake()
    {
        switch (_lockState)
        {
            case lockStates.lock30:
                QualitySettings.vSyncCount = 0;  
                Application.targetFrameRate = 30;
                break;
            case lockStates.lock60:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 60;
                break;
            case lockStates.lock120:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 120;
                break;
            case lockStates.unlock:
                QualitySettings.vSyncCount = 0;
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
