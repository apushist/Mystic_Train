using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fog : MonoBehaviour
{
    [SerializeField] Tilemap _tm;
    [SerializeField] Transform _startPos;
    [SerializeField] GameObject _fogPrefab;
    [SerializeField] int _fillDelay = 1;
    [SerializeField] int _deathColActivateDelay = 1;

    private HashSet<Vector3Int> _poses;
    private UnityEngine.Vector3 _gridOffset;
    Vector3Int startCell;
    void Start()
    {
        _gridOffset = GetComponentInChildren<Grid>().transform.GetChild(0).transform.position;
        _poses = new HashSet<Vector3Int>();
        startCell = _tm.WorldToCell(_startPos.position);
        if (!_tm.GetTile(startCell))
        {
            Debug.Log("not valid start position!!!(((");
        }
        //StartFillFog(currentCell);
    }
    public void StartFillFog(Vector3Int currentCell)
    {
        InstantFogPoint(currentCell);
        _poses.Add(currentCell);
        StartCoroutine(FillRingRecWithDelay4(currentCell));
    }

    public IEnumerator FillRingRecWithDelay(Vector3Int vecCur)
    {
        yield return new WaitForSeconds(_fillDelay);
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (!(i == 0 && j == 0))
                {
                    Vector3Int vecCurNext = new Vector3Int(vecCur.x + i, vecCur.y + j, vecCur.z);
                    if (_tm.GetTile(vecCurNext) && !_poses.Contains(vecCurNext))
                    {
                        InstantFogPoint(vecCurNext);
                        _poses.Add(vecCurNext);
                        StartCoroutine(FillRingRecWithDelay(vecCurNext));
                    }
                }
            }
        }
    }
    public IEnumerator FillRingRecWithDelay4(Vector3Int vecCur)
    {
        yield return new WaitForSeconds(_fillDelay);
        for (int i = -1; i <= 1; i += 2)
        {
            Vector3Int vecCurNext = new Vector3Int(vecCur.x + i, vecCur.y, vecCur.z);
            if (_tm.GetTile(vecCurNext) && !_poses.Contains(vecCurNext))
            {
                InstantFogPoint(vecCurNext);
                _poses.Add(vecCurNext);
                StartCoroutine(FillRingRecWithDelay4(vecCurNext));
            }
            vecCurNext = new Vector3Int(vecCur.x, vecCur.y + i, vecCur.z);
            if (_tm.GetTile(vecCurNext) && !_poses.Contains(vecCurNext))
            {
                InstantFogPoint(vecCurNext);
                _poses.Add(vecCurNext);
                StartCoroutine(FillRingRecWithDelay4(vecCurNext));
            }

        }
    }

    void InstantFogPoint(Vector3Int vector)
    {
        var ins = Instantiate(_fogPrefab);
        ins.transform.position = new UnityEngine.Vector3(vector.x + 0.5f, vector.y + 0.5f, vector.z) + _gridOffset;
        var deathCol = ins.GetComponent<Collider2D>();
        deathCol.enabled = false;
        StartCoroutine(ActivateDeathCollider(deathCol));

    }

    IEnumerator ActivateDeathCollider(Collider2D col)
    {
        yield return new WaitForSeconds(_deathColActivateDelay);
        col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartFillFog(startCell);
        }
    }
}
