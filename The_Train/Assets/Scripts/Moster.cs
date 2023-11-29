using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class Moster : MonoBehaviour
{
    public GameObject monsterTexture;
	public PlayerController controller;
	private Animator anim;


	// Start is called before the first frame update
	void Start()
	{
		anim = monsterTexture.GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		anim.SetTrigger("ActivateMonster");
	}
}
