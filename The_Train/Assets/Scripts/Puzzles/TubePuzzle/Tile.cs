using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    internal Vector2 _index;
    [SerializeField] private float _rotationFrames;
    [SerializeField] internal float _fillFrames;
    [SerializeField] internal bool _clickable = true;
    [SerializeField] private RectTransform _tube;
    [SerializeField] internal Image _tubeContent;
    [SerializeField] internal Direction inputDir;
    [SerializeField] internal Direction outputDir;

    internal bool inRotate = false;

    public void Init(float _tileSize, Vector2 ind)
    {
        var rect = GetComponent<RectTransform>();
        rect.localScale = new Vector2(_tileSize, _tileSize) / (rect.offsetMax.x - rect.offsetMin.x);
        ResetThisTile();
        _index = ind;
        rotationRandomize();

    }
    public void OnButtonClick()
    {
        if (_clickable)
        {
            StartCoroutine(rotationLerp(_rotationFrames));
        }
    }

    IEnumerator rotationLerp(float frames)
    {
        _clickable = false;
        inRotate = true;
        for(int i = 0; i < frames; i++)
        {
            _tube.Rotate(new Vector3(0, 0, -90 / frames));
            yield return new WaitForFixedUpdate();
        }
        ChangeDirection();

        _clickable = true;
        inRotate = false;
    }
    public IEnumerator FillThis(float frames)
    {
        _clickable = false;
        for (int i = 0; i < frames; i++)
        {
            _tubeContent.fillAmount = 1f / frames * (i+1);
            yield return new WaitForFixedUpdate();
        }
        TubePuzzle.instance.FillNextTile(_index, outputDir);
    }
    void rotationRandomize()
    {
        int rand = Random.Range(0, 4);
        for (int i = 0; i < rand; i++)
        {
            rotationOnce();
            ChangeDirection();
        }
        _clickable = true;
    }
    public void rotationOnce()
    {
        _tube.Rotate(new Vector3(0, 0, -90));
    }
    public void ResetThisTile()
    {
        _tubeContent.fillAmount = 0;
        _clickable = true;
        rotationRandomize();
    }

    public virtual void StartFilling() { }
    public virtual bool CheckInputDirectionValid(Direction input)
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
    public void ChangeDirection()
    {
        inputDir = GetNextDirection(inputDir);
        outputDir = GetNextDirection(outputDir);
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

