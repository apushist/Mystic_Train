using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private float _tileSize;

    //[SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;

    public static GridManager instance;

    private void Awake()
    {
        instance = this;
    }
    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x + 0.5f, 0, y + 0.5f) * _tileSize + transform.position, Quaternion.Euler(90, 0, 0), this.transform);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.Init(_tileSize);

                _tiles[new Vector2(spawnedTile.transform.position.x, spawnedTile.transform.position.z)] = spawnedTile;
            }

        }
        Debug.Log("State::GenerateGrid");
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

}

