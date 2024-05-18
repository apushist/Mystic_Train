using UnityEngine;

public class RemoveItemTrigger : MonoBehaviour
{
    [SerializeField] int _id;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.CompareTo("Player")==0)
        {
            Inventory.instance.RemoveItem(_id);
            gameObject.SetActive(false);
        }
    }
}
