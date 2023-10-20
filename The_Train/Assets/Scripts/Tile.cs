using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _highlightEnter;
    [SerializeField] private Vector2 _index;
    [SerializeField] private float _rotationTime;
    [SerializeField] private float _rotationFrames;
    [SerializeField] private bool _clickable = true;
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
        if (_clickable)
        {
            //_tube.Rotate(new Vector3(0, 0, 90));
            StartCoroutine(rotationLerp(_rotationTime, _rotationFrames));
            GridManager.instance.PrintTouched(_index);
        }
    }

    IEnumerator rotationLerp(float time, float frames)
    {
        _clickable = false;
        for(int i = 0; i < frames; i++)
        {
            _tube.Rotate(new Vector3(0, 0, -90 / frames));
            yield return new WaitForSeconds(time / frames);
        }
        _clickable = true;
    }
}

