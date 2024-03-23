using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCountTrigger : MonoBehaviour
{
    [SerializeField] Transform[] _tpPositions;
    [SerializeField] MonsterFinder _monster;
    bool[] _itemsCheck;
    int _lastItem;
    void Start()
    {
        _itemsCheck = new bool[_tpPositions.Length];
    }
    public void AddItem(int i)
    {
        _itemsCheck[i] = true;
        _lastItem = i;
        if (CheckAll())
        {
            TriggerMonster();
        }
    }

    private void TriggerMonster()
    {
        _monster.transform.position = _tpPositions[_lastItem].position;
        _monster.StartMonsterRage();
    }

    public bool CheckAll()
    {
        foreach(var i in _itemsCheck)
        {
            if (!i)
            {
                return false;
            }
        }
        return true;
    }
}
