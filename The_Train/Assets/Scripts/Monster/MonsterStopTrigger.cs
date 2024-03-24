using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStopTrigger : MonoBehaviour
{
    [SerializeField] MonsterFinder _monster;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<MonsterFinder>()!=null)
        {
            _monster.OnMonsterDeath();
            gameObject.SetActive(false);
        }
    }
}
