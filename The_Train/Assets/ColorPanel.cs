using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPanel : MonoBehaviour
{
    public PanelRotateState _state;

    internal void NextState()
    {
        if (_state == PanelRotateState.left)
            _state = PanelRotateState.up;
        else
            _state++;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
public enum PanelRotateState { up, right, down, left }
