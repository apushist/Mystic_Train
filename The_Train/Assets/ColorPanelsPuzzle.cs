using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPanelsPuzzle : PuzzleBase
{
    [SerializeField] private GameObject _puzzleScreen;
    [SerializeField] private RectTransform _parentSprite;
    [SerializeField] private RectTransform _dragSpace;

    [SerializeField] private RectTransform[] _verticals; ////hide
    [SerializeField] private RectTransform[] _gorizontals; //hide    

    [SerializeField] internal Sprite[] _itemsReference;

    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _goodColor;
    [SerializeField] private Color _badColor;

    public static ColorPanelsPuzzle instance;

    Vector2[,] _colorPoints;

    float _orientWidth = 1920;
    float _orientHeight = 1080;


    Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    Vector2 canvasParentMin;
    Vector2 canvasParentMax;

    RectTransform _draggingPanel;
    Vector2 _draggingOffset;
    bool _inRotation = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        canvasParentMin = CanvasToMousePoint(_parentSprite.anchoredPosition + _parentSprite.offsetMin);
        canvasParentMax = CanvasToMousePoint(_parentSprite.anchoredPosition + _parentSprite.offsetMax);

        _colorPoints = new Vector2[_itemsReference.Length, _itemsReference.Length];
        for(int i = 0; i < _itemsReference.Length; i++)
        {
            for (int j = 0; j < _itemsReference.Length; j++)
            {
                _colorPoints[i, j] = new Vector2(-1, -1);
            }
        }
        _gorizontals = new RectTransform[_itemsReference.Length];
        _verticals = new RectTransform[_itemsReference.Length];
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
        if (!obj.CompareTag("ColorPanel") || _draggingPanel!=null) return;

        if (IsInParentRect(V2(Input.mousePosition))&& IsPanelInLogic(obj.GetComponent<RectTransform>()))
        {
            var colorPanel = obj.GetComponent<ColorPanel>();
            if (colorPanel._state == PanelRotateState.right || colorPanel._state == PanelRotateState.left)
            {
                var yLerpCount = lerpParentCoordsToCountY(Input.mousePosition.y);
                if (_gorizontals[_gorizontals.Length - yLerpCount - 1] != null)
                {
                    ClearMatrixItemData(yLerpCount, obj.GetComponent<RectTransform>());
                }
            }
            else
            {
                var xLerpCount = lerpParentCoordsToCountX(Input.mousePosition.x);
                if (_verticals[xLerpCount] != null)
                {
                    ClearMatrixItemData(xLerpCount, obj.GetComponent<RectTransform>());
                }
            }
            UpdateColors();
        }
        _draggingOffset = CanvasToMousePoint(obj.GetComponent<RectTransform>().anchoredPosition) - V2(Input.mousePosition);
        _draggingPanel = Instantiate(obj, _dragSpace).GetComponent<RectTransform>();
        ResetPanelColor(_draggingPanel);

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
                if (_gorizontals[_gorizontals.Length - yLerpCount - 1] == null)
                {
                    inst.anchoredPosition = new Vector2(0, lerpParentCoordsY(yLerpCount));
                    SetMatrixItemData(yLerpCount, inst);
                }
                else
                {
                    inst.anchoredPosition = MousePointToCanvas(V2(Input.mousePosition) + _draggingOffset);
                }
            }
            else
            {
                var xLerpCount = lerpParentCoordsToCountX(Input.mousePosition.x);
                if (_verticals[xLerpCount] == null)
                {
                    inst.anchoredPosition = new Vector2(lerpParentCoordsX(xLerpCount), 0);
                    SetMatrixItemData(xLerpCount, inst);
                }
                else
                {
                    inst.anchoredPosition = MousePointToCanvas(V2(Input.mousePosition) + _draggingOffset);
                }
            }
            UpdateColors();
        }
        else
        {
            inst.anchoredPosition = MousePointToCanvas(V2(Input.mousePosition) + _draggingOffset);
        }

        Destroy(_draggingPanel.gameObject);
        if (CheckForWin())
        {
            Debug.Log("win");
        }
    }

    public void MovePanel()
    {
        _draggingPanel.position = V2(Input.mousePosition) + _draggingOffset;
    }

    public void SetMatrixItemData(int lerpCount, RectTransform obj)
    {
        var colorPanel = obj.transform.GetComponent<ColorPanel>();
        if (colorPanel._state == PanelRotateState.right || colorPanel._state == PanelRotateState.left)
        {
            _gorizontals[_gorizontals.Length - lerpCount - 1] = obj;
            if(colorPanel._state == PanelRotateState.right)
            {
                for(int i = 0; i < _gorizontals.Length; i++)
                {
                    _colorPoints[i, _gorizontals.Length - lerpCount - 1].x = colorPanel._itemsIndex[i];
                }
            }
            else
            {
                for (int i = 0; i < _gorizontals.Length; i++)
                {
                    _colorPoints[i, _gorizontals.Length-lerpCount-1].x = colorPanel._itemsIndex[_gorizontals.Length - i - 1];
                }
            }
        }
        else
        {
            _verticals[lerpCount] = obj;
            if (colorPanel._state == PanelRotateState.up)
            {
                for (int i = 0; i < _verticals.Length; i++)
                {
                    _colorPoints[lerpCount, i].y = colorPanel._itemsIndex[i];
                }
            }
            else
            {
                for (int i = 0; i < _verticals.Length; i++)
                {
                    _colorPoints[lerpCount, i].y = colorPanel._itemsIndex[_gorizontals.Length - i - 1];
                }
            }
        }
    }
    public void ClearMatrixItemData(int lerpCount, RectTransform obj)
    {
        var colorPanel = obj.transform.GetComponent<ColorPanel>();
        if (colorPanel._state == PanelRotateState.right || colorPanel._state == PanelRotateState.left)
        {
            for (int i = 0; i < _gorizontals.Length; i++)
            {
                _colorPoints[i, _gorizontals.Length - lerpCount - 1].x = -1;
            }
            _gorizontals[_gorizontals.Length - lerpCount - 1] = null;
        }
        else
        {

            for (int i = 0; i < _verticals.Length; i++)
            {
                _colorPoints[lerpCount, i].y = -1;
            }

            _verticals[lerpCount] = null;
        }
    }

    public void UpdateColors()
    {
        for (int j = 0; j < _itemsReference.Length; j++)
        {
            var pan = _gorizontals[j];
            if (pan == null) continue;
            var colorPanel = pan.GetComponent<ColorPanel>();
            if (colorPanel._state == PanelRotateState.right)
            {
                for (int i = 0; i < _itemsReference.Length; i++)
                {
                    var v2 = _colorPoints[i, j];
                    if (v2.x == -1 || v2.y == -1)
                        colorPanel._itemsObject[i].GetComponent<Image>().color = _defaultColor;
                    else if (v2.x == v2.y)
                        colorPanel._itemsObject[i].GetComponent<Image>().color = _goodColor;
                    else
                        colorPanel._itemsObject[i].GetComponent<Image>().color = _badColor;
                }
            }
            else if (colorPanel._state == PanelRotateState.left)
            {
                for (int i = 0; i < _itemsReference.Length; i++)
                {
                    var v2 = _colorPoints[i, j];
                    if (v2.x == -1 || v2.y == -1)
                        colorPanel._itemsObject[_itemsReference.Length - i - 1].GetComponent<Image>().color = _defaultColor;
                    else if (v2.x == v2.y)
                        colorPanel._itemsObject[_itemsReference.Length - i - 1].GetComponent<Image>().color = _goodColor;
                    else
                        colorPanel._itemsObject[_itemsReference.Length - i - 1].GetComponent<Image>().color = _badColor;
                }
            }

        }
        for (int j = 0; j < _itemsReference.Length; j++)
        {
            var pan = _verticals[j];
            if (pan == null) continue;
            var colorPanel = pan.GetComponent<ColorPanel>();
            if (colorPanel._state == PanelRotateState.up)
            {
                for (int i = 0; i < _itemsReference.Length; i++)
                {
                    var v2 = _colorPoints[j, i];
                    if (v2.x == -1 || v2.y == -1)
                        colorPanel._itemsObject[i].GetComponent<Image>().color = _defaultColor;
                    else if (v2.x == v2.y)
                        colorPanel._itemsObject[i].GetComponent<Image>().color = _goodColor;
                    else
                        colorPanel._itemsObject[i].GetComponent<Image>().color = _badColor;
                }
            }
            else if (colorPanel._state == PanelRotateState.down)
            {
                for (int i = 0; i < _itemsReference.Length; i++)
                {
                    var v2 = _colorPoints[j, i];
                    if (v2.x == -1 || v2.y == -1)
                        colorPanel._itemsObject[_itemsReference.Length - i - 1].GetComponent<Image>().color = _defaultColor;
                    else if (v2.x == v2.y)
                        colorPanel._itemsObject[_itemsReference.Length - i - 1].GetComponent<Image>().color = _goodColor;
                    else
                        colorPanel._itemsObject[_itemsReference.Length - i - 1].GetComponent<Image>().color = _badColor;
                }
            }

        }
    }
    public void ResetPanelColor(RectTransform pan)
    {
        var colorPanel = pan.GetComponent<ColorPanel>();
        for (int i = 0; i < _itemsReference.Length; i++)
        {
            colorPanel._itemsObject[i].GetComponent<Image>().color = _defaultColor;
        }
    }

    //возвращает кэнввэс координату привязки по номеру для ВЕРТИКАЛЬНОЙ планки
    public float lerpParentCoordsX(int v)
    {
        float parentSizeMouse = (canvasParentMax.x - canvasParentMin.x);
        return (v - 2) * (parentSizeMouse / 5) * _orientWidth / Screen.width;
    }
    //возвращает кэнввэс координату привязки по номеру для ГОРИЗОНТАЛЬНОЙ планки
    public float lerpParentCoordsY(int v)
    {
        float parentSizeMouse = (canvasParentMax.y - canvasParentMin.y);
        return (v - 2) * (parentSizeMouse / 5) * _orientWidth / Screen.width;
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

    private bool IsPanelInLogic(RectTransform r)
    {
        foreach(var i in _verticals)
        {
            if (i == r) return true;
        }
        foreach (var i in _gorizontals)
        {
            if (i == r) return true;
        }
        return false;
    }
    private bool CheckForWin()
    {
        for (int i = 0; i < _itemsReference.Length; i++)
        {
            for (int j = 0; j < _itemsReference.Length; j++)
            {
                var cur = _colorPoints[i, j];
                if (cur.x != cur.y || cur.x==-1 || cur.y == -1)
                    return false;
            }
        }
        return true;
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
