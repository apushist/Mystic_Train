using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCountTrigger : MonoBehaviour
{
    [SerializeField] Transform[] _tpPositions;
    [SerializeField] MonsterFinder _monster;
    [SerializeField] SaveSystemObject _saveSystemObject;
    bool[] _itemsCheck;
    int _lastItem;
    bool _triggerMonster;

    void Start()
    {
        _itemsCheck = new bool[_tpPositions.Length];
        _triggerMonster = false;
    }
    public void AddItem(int i)
    {
        _itemsCheck[i] = true;
        _lastItem = i;
        if (CheckAll())
        {
            _saveSystemObject.SaveFunction();
            _triggerMonster = true;
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

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("Player") && _triggerMonster)
        {
			TriggerMonster();
		}
	}
}
