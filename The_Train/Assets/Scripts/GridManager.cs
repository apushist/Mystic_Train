
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
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
    public void PrintTouched(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
            Debug.Log(pos.x + ", " + pos.y);
        else
            Debug.Log("Tile find error");
        
    }

}

