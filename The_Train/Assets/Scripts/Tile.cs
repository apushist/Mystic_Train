using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _highlightEnter;
    private Collider _col;
    public TileState currentState;

    public void Init(float _tileSize)
    {
        transform.localScale = new Vector3(_tileSize, _tileSize, _tileSize);

    }
}
public enum TileState
{
    empty,
    close,
    obstacle
}
