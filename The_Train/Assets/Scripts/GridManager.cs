
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tile[] _tilePrefab;
    [SerializeField] private float _tileSize;
    [SerializeField] private float _tileOffset;
    [SerializeField] private float _endOffset;
    [SerializeField] private int _tileCount;
    [SerializeField] private RectTransform _parentSprite;

    [Header("DestinationSettings")]
    [SerializeField] private Vector2 _startPoint;
    [SerializeField] private Vector2 _endPoint;
    [SerializeField] private Direction _startInput;
    [SerializeField] private Direction _endOutput;

    private Dictionary<Vector2, Tile> _tiles;
    public static GridManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _tileSize = ((_parentSprite.offsetMax.x - _parentSprite.offsetMin.x) - (_endOffset * 2) - (_tileOffset * (_tileCount - 1))) / _tileCount;
        GenerateGrid();
    }
    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _tileCount; x++)
        {
            for (int y = 0; y < _tileCount; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab[Random.Range(0,_tilePrefab.Length)], _parentSprite);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * (_tileSize + _tileOffset) + _endOffset, y * (_tileSize + _tileOffset) + _endOffset);
                spawnedTile.Init(_tileSize, new Vector2(x,y));

                _tiles[new Vector2(x, y)] = spawnedTile;
            }

        }
        Debug.Log("State::GenerateGrid");

        Tile nextTile = GetNextTile(_startPoint, _startInput);
        nextTile.StartInflate();
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
    public void InflateNextTile(Vector2 pos, Direction output)
    {
        Tile nextTile = GetNextTile(pos, output);
        nextTile.StartInflate();
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
        if (pos == _endPoint)
        {
            Debug.Log("win");
            return null;
        }
        if (_tiles.TryGetValue(nextPos, out var tile))
        {
            nextTile = tile;
            if (nextTile.CheckInputDirectionValid(nextInput))
            {
                return nextTile;
            }
            else
            {
                Debug.Log("critical damage... (next tile doesnt have this direction)");
                return null;
            }
        }
        else
        {
            Debug.Log("critical damage... (next tile undefined)");
            return null;
        }
        
    }

    public void PrintTouched(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
            Debug.Log(pos.x + ", " + pos.y);
        else
            Debug.Log("Tile find error");
        
    }

}

