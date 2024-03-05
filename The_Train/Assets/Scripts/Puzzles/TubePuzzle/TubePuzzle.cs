
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TubePuzzle : PuzzleBase
{
    [SerializeField] private GameObject _puzzleScreen;
    [SerializeField] private Tile[] _tilePrefab;
    private float _tileSize;
    [SerializeField] private float _tileOffset;
    [SerializeField] private float _endOffset;
    [SerializeField] private int _tileCount;
    [SerializeField] private RectTransform _parentSprite;
    [SerializeField] private RectTransform _startTile;
    [SerializeField] private RectTransform _endTile;
    [SerializeField] private TextMeshProUGUI _startTimeCounter;

    [Header("DestinationSettings")]
    [SerializeField] private int _onStartDelay;
    [SerializeField] private Vector2 _startPoint;
    [SerializeField] private Vector2 _endPoint;
    [SerializeField] private Direction _startInput;
    [SerializeField] private Direction _endOutput;

    [Header("MapSettings")]
    [SerializeField] bool _generateWithData = false;
    private int[] _data6; 

    private Dictionary<Vector2, Tile> _tiles;
    public static TubePuzzle instance;

    private void Awake()
    {
        instance = this;
        EnableThisPuzzle(false);
        _data6 = new int[36]
        {0,1,0,1,0,0,
        0,1,0,0,0,1,
        0,0,0,1,0,1,
        0,1,1,0,0,1,
        1,1,0,0,1,1,
        0,1,1,1,1,0};
        _tileSize = ((_parentSprite.offsetMax.x - _parentSprite.offsetMin.x) - (_endOffset * 2) - (_tileOffset * (_tileCount - 1))) / _tileCount;
        
    }
    public override void StartPuzzle()
    {
        EnableThisPuzzle(true);        
        InitStartEnd();
        GenerateGrid();
        StartCoroutine(StartFilling(_onStartDelay));
    }

    private void EnableThisPuzzle(bool isOn)
    {
        _puzzleScreen.SetActive(isOn);
    }
    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _tileCount; x++)
        {
            for (int y = 0; y < _tileCount; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab[_generateWithData ? _data6[x*_tileCount+y] :Random.Range(0,_tilePrefab.Length)], _parentSprite);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * (_tileSize + _tileOffset) + _endOffset, y * (_tileSize + _tileOffset) + _endOffset);
                spawnedTile.Init(_tileSize, new Vector2(x,y));

                _tiles[new Vector2(x, y)] = spawnedTile;
            }

        }
        
    }
    void InitStartEnd()
    {
        _startTile.localScale = new Vector2(_tileSize, _tileSize) / (_startTile.offsetMax.x - _startTile.offsetMin.x);
        _endTile.localScale = new Vector2(_tileSize, _tileSize) / (_endTile.offsetMax.x - _endTile.offsetMin.x);
        _startTile.anchoredPosition = new Vector2(_startPoint.x * (_tileSize + _tileOffset) + _endOffset, _startPoint.y * (_tileSize + _tileOffset) + _endOffset);
        _endTile.anchoredPosition = new Vector2(_endPoint.x * (_tileSize + _tileOffset) + _endOffset, _endPoint.y * (_tileSize + _tileOffset) + _endOffset);
        switch (_startInput)
        {
            case Direction.up:
                _startTile.Rotate(new Vector3(0, 0, -90));
                break;
            case Direction.right:
                _startTile.Rotate(new Vector3(0, 0, 0));
                break;
            case Direction.down:
                _startTile.Rotate(new Vector3(0, 0, 90));
                break;
            case Direction.left:
                _startTile.Rotate(new Vector3(0, 0, 180));
                break;
            default:
                break;
        }
        switch (_endOutput)
        {
            case Direction.up:
                _startTile.Rotate(new Vector3(0, 0, -90));
                break;
            case Direction.right:
                _startTile.Rotate(new Vector3(0, 0, -180));
                break;
            case Direction.down:
                _startTile.Rotate(new Vector3(0, 0, -270));
                break;
            case Direction.left:
                _startTile.Rotate(new Vector3(0, 0, 0));
                break;
            default:
                break;
        }
    }
    void ResetFillProgress()
    {
        for (int x = 0; x < _tileCount; x++)
        {
            for (int y = 0; y < _tileCount; y++)
            {
                if(_tiles.TryGetValue(new Vector2(x, y), out var tile))
                {
                    Destroy(tile.gameObject);
                }
            }
        }
        _tiles.Clear();
    }
    IEnumerator StartFilling(int time)
    {
        for(int i = time; i > 0; i--)
        {
            _startTimeCounter.text = i.ToString();
            yield return new WaitForSeconds(1f);
            Debug.Log(i);
        }
        _startTimeCounter.text = "!!!";
        FillNextTile(_startPoint, _startInput);
    }
    public void FillNextTile(Vector2 pos, Direction output)
    {
        Tile nextTile = GetNextTile(pos, output);
        if (nextTile == null) return;
        nextTile.StartFilling();
    }
    public Tile GetNextTile(Vector2 pos, Direction output)
    {
        Tile nextTile;
        Vector2 nextPos;
        Direction nextInput;
        switch (output)
        {
            case Direction.up:
                nextPos = new Vector2(pos.x, pos.y + 1);
                nextInput = Direction.down;
                break;
            case Direction.right:
                nextPos = new Vector2(pos.x+1, pos.y);
                nextInput = Direction.left;
                break;
            case Direction.down:
                nextPos = new Vector2(pos.x, pos.y - 1);
                nextInput = Direction.up;
                break;
            case Direction.left:
                nextPos = new Vector2(pos.x-1, pos.y);
                nextInput = Direction.right;
                break;
            default:
                nextPos = new Vector2(0,0);
                nextInput = Direction.right;
                Debug.Log("Exception direction find");
                break;
        }
        if (nextPos == _endPoint)
        {
            WinPuzzle();
            return null;
        }
        if (_tiles.TryGetValue(nextPos, out var tile))
        {
            nextTile = tile;
            if (nextTile.CheckInputDirectionValid(nextInput) && !nextTile.inRotate)
            {
                return nextTile;
            }
            else
            {
                Debug.Log($"next tile doesnt have this direction: {nextTile.inputDir}, {nextTile.outputDir}; excepted: {nextInput}");
                Debug.Log($"position excepted: {nextPos.x}, {nextPos.y}");
                LoosePuzzle();
                return null;
            }
        }
        else
        {            
            Debug.Log($"next tile undefined by position; excepted: {nextPos.x}, {nextPos.y}");
            LoosePuzzle();
            return null;
        }
        
    }
    public override void WinPuzzle()
    {
        ResetFillProgress();
        EnableThisPuzzle(false);
        PuzzlesContoller.instance.Win();
    }
    public override void LoosePuzzle()
    {
        StopAllCoroutines();
        ResetFillProgress();
        EnableThisPuzzle(false);
        PuzzlesContoller.instance.Loose();
    }
    public void PrintTouched(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
            Debug.Log(pos.x + ", " + pos.y);
        else
            Debug.Log("Tile find error");
        
    }
    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

}

