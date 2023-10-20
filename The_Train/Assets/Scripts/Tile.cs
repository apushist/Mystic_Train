using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _highlightEnter;
    [SerializeField] private Vector2 _index;
    RectTransform _tube;

    public void Init(float _tileSize, Vector2 ind)
    {
        var rect = GetComponent<RectTransform>();
        rect.localScale = new Vector2(_tileSize, _tileSize) / (rect.offsetMax.x - rect.offsetMin.x);
        _tube = transform.GetChild(0).GetComponent<RectTransform>();
        _index = ind;

    }
    public void OnButtonClick()
    {
        _tube.Rotate(new Vector3(0, 0, 90));
        GridManager.instance.PrintTouched(_index);
    }
}

