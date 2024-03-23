using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRunTrigger : MonoBehaviour
{
    [SerializeField] MonsterFinder _monster;
    void Awake()
    {
        _monster.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _monster.OnMonsterAlive();
            gameObject.SetActive(false);
        }
    }
}
