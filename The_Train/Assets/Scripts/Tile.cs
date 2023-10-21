using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private Vector2 _index;
    [SerializeField] private float _rotationTime;
    [SerializeField] private float _rotationFrames;
    [SerializeField] internal float _inflationTime;
    [SerializeField] internal float _inflationFrames;
    [SerializeField] internal bool _clickable = true;
    [SerializeField] private RectTransform _tube;
    [SerializeField] internal Image _tubeContent;
    [SerializeField] internal Direction inputDir;
    [SerializeField] internal Direction outputDir;

    public void Init(float _tileSize, Vector2 ind)
    {
        var rect = GetComponent<RectTransform>();
        rect.localScale = new Vector2(_tileSize, _tileSize) / (rect.offsetMax.x - rect.offsetMin.x);
        _tubeContent.fillAmount = 0;
        _index = ind;
        rotationRandomize();

    }
    public void OnButtonClick()
    {
        if (_clickable)
        {
            StartCoroutine(rotationLerp(_rotationTime, _rotationFrames));
            //GridManager.instance.PrintTouched(_index);
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
        inputDir = GetNextDirection(inputDir);
        outputDir = GetNextDirection(outputDir);
        Debug.Log(inputDir + " " + outputDir + " ||| " + _index.x + " " + _index.y);
        _clickable = true;
    }
    public IEnumerator inflation(float time, float frames)
    {
        _clickable = false;
        for (int i = 0; i < frames; i++)
        {
            _tubeContent.fillAmount = 1f / frames * (i+1);
            yield return new WaitForSeconds(time / frames);
        }
        Debug.Log("inflated: " + _index.x + " " + _index.y);
        GridManager.instance.InflateNextTile(_index, outputDir);
    }
    void rotationRandomize()
    {
        int rand = Random.Range(0, 4);
        for (int i = 0; i < rand; i++)
        {
            _tube.Rotate(new Vector3(0, 0, -90));
            inputDir = GetNextDirection(inputDir);
            outputDir = GetNextDirection(outputDir);            
        }
        _clickable = true;
    }

    public virtual void StartInflate() { }

    public bool CheckInputDirectionValid(Direction input)
    {
        if (inputDir == input) return true;
        else if(outputDir == input)
        {
            Direction toSwap = inputDir;
            inputDir = outputDir;
            outputDir = toSwap;
            return true;
        }
        else
        {
            return false;
        }
    }
    Direction GetNextDirection(Direction cur)
    {
        switch (cur)
        {
            case Direction.up:
                return Direction.right;
            case Direction.right:
                return Direction.down;
            case Direction.down:
                return Direction.left;
            case Direction.left:
                return Direction.up;
            default:
                return cur++;
        }
    }
}
public enum Direction
{
    right = 1,
    down = 2,
    left = 3,
    up = 4
}

