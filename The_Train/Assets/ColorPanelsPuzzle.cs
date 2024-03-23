using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorPanelsPuzzle : PuzzleBase
{
    [SerializeField] private GameObject _puzzleScreen;
    [SerializeField] private RectTransform _parentSprite;
    [SerializeField] private RectTransform _dragSpace;

    [SerializeField] private GameObject[] _verticals;
    [SerializeField] private GameObject[] _gorizontals;

    public static ColorPanelsPuzzle instance;

    Vector2[][] _colorPoints;

    float _orientWidth = 1920;
    float _orientHeight = 1080;


    Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    Vector2 canvasParentMin;
    Vector2 canvasParentMax;

    RectTransform _draggingPanel;
    Vector2 _draggingOffset;
    bool _inRotation = false;

    private void Start()
    {
        instance = this;
        canvasParentMin = CanvasToMousePoint(_parentSprite.anchoredPosition + _parentSprite.offsetMin);
        canvasParentMax = CanvasToMousePoint(_parentSprite.anchoredPosition + _parentSprite.offsetMax);
    }
    private void Update()
    {
        if (_draggingPanel != null)
        {
            MovePanel();
        }
        if ((Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0)) && _draggingPanel != null && !_inRotation)
        {
            DropPanel();
        }
        if (Input.GetMouseButtonUp(1) && _draggingPanel != null && !_inRotation)
        {
            RotatePanel();
        }
    }

    public Vector2 CanvasToMousePoint(Vector2 v)
    {
        return v / _orientWidth * Screen.width + screenCenter;
    }
    public Vector2 MousePointToCanvas(Vector2 v)
    {
        return (v - screenCenter) * _orientWidth / Screen.width;
    }
    public Vector2 V2(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }
    public Vector3 V3(Vector2 v)
    {
        return new Vector3(v.x, v.y, 0);
    }

    public bool IsInParentRect(Vector2 v)
    {
        return v.x > canvasParentMin.x && v.x < canvasParentMax.x && v.y > canvasParentMin.y && v.y < canvasParentMax.y;
    }

    public void GetPanelToDrag(GameObject obj)
    {
        if (!obj.CompareTag("ColorPanel")) return;

        _draggingOffset = CanvasToMousePoint(obj.GetComponent<RectTransform>().anchoredPosition) - V2(Input.mousePosition);
        _draggingPanel = Instantiate(obj, _dragSpace).GetComponent<RectTransform>();

        Destroy(obj);
    }

    public void DropPanel()
    {
        if (_inRotation)
        {
            StopAllCoroutines();
            _inRotation = false;
        }
        var inst = Instantiate(_draggingPanel, _parentSprite);
        
        if (IsInParentRect(V2(Input.mousePosition)))
        {
            var colorPanel = _draggingPanel.transform.GetComponent<ColorPanel>();
            if (colorPanel._state == PanelRotateState.right || colorPanel._state == PanelRotateState.left)
            {
                var yLerpCount = lerpParentCoordsToCountY(Input.mousePosition.y);
                inst.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, lerpParentCoordsY(yLerpCount));
            }
            else
            {
                var xLerpCount = lerpParentCoordsToCountX(Input.mousePosition.x);
                inst.GetComponent<RectTransform>().anchoredPosition = new Vector2(lerpParentCoordsX(xLerpCount), 0);
            }
           
        }
        else
        {
            inst.GetComponent<RectTransform>().anchoredPosition = MousePointToCanvas(V2(Input.mousePosition) + _draggingOffset);
        }

        Destroy(_draggingPanel.gameObject);
    }
    public void MovePanel()
    {
        _draggingPanel.position = V2(Input.mousePosition) + _draggingOffset;
    }

    //возвращает кэнввэс координату привязки по номеру
    public float lerpParentCoordsX(int v)
    {
        float parentSizeMouse = (canvasParentMax.x - canvasParentMin.x);
        return (v - 2) * (parentSizeMouse / 5) * _orientWidth / Screen.width;
    }
    public float lerpParentCoordsY(int v)
    {
        float parentSizeMouse = (canvasParentMax.y - canvasParentMin.y);
        return (v - 2) * (parentSizeMouse / 5) * _orientWidth / Screen.width;
    }
    public float lerpParentCoordsН(int v)
    {
        float parentSizeMouse = (canvasParentMax.y - canvasParentMin.y);
        return (v - 2) * (parentSizeMouse / 5) * _orientHeight / Screen.height;
    }
    //возвращает номер привязки панели(0-4)
    public int lerpParentCoordsToCountX(float v)
    {
        float parentSizeMouse = (canvasParentMax.x - canvasParentMin.x);
        return (int)(v - (Screen.width - parentSizeMouse) / 2) / (int)(parentSizeMouse / 5);
    }
    public int lerpParentCoordsToCountY(float v)
    {
        float parentSizeMouse = (canvasParentMax.y - canvasParentMin.y);
        return (int)(v - (Screen.height - parentSizeMouse) / 2) / (int)(parentSizeMouse / 5);
    }

    private void RotatePanel()
    {
        _inRotation = true;
        _draggingPanel.transform.GetComponent<ColorPanel>().NextState();
        StartCoroutine(RotatePanelIE());
    }

    IEnumerator RotatePanelIE()
    {
        int frames = 9;
        Vector2 rotationVector = new Vector2(_draggingOffset.y * -1, _draggingOffset.x) - _draggingOffset;
        for (int i = 0; i < frames; i++)
        {
            _draggingPanel.Rotate(new Vector3(0, 0, 90 / frames));
            _draggingOffset += rotationVector / frames;
            yield return new WaitForFixedUpdate();
        }
        _inRotation = false;
    }

}
