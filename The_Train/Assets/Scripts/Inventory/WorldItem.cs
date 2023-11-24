using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private GameObject _effect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Inventory.instance.AddItem(_id);
            gameObject.SetActive(false);
            PlayEffect();
        }
    }
    void PlayEffect()
    {
        var ef = Instantiate(_effect, transform.position, Quaternion.identity);
        ef.GetComponent<ParticleSystem>().Play();
        Destroy(ef, 5);

    }
}
