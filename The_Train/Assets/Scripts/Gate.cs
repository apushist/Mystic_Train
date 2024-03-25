using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public GameObject objectToDestroy;
    public bool isLocked;

    private Animator animator;

	private void Start()
	{
		animator = GetComponentInChildren<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("Player") && !isLocked)
        {
            animator.SetTrigger("Open");
            Destroy(objectToDestroy);
        }
    }
}
