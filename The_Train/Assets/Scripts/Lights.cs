using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lights : MonoBehaviour
{
	public GameObject lights;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		foreach(Light2D l in lights.GetComponentsInChildren<Light2D>())
		{
			l.enabled = false;
		}
	}
}
