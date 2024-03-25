using UnityEngine;

public class MonsterTriggerElement : MonoBehaviour
{
    [SerializeField] MonsterCountTrigger _trigger;
    [SerializeField] int _index;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _trigger.AddItem(_index);
        }
    }
}
