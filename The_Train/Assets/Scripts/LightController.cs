using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum ActionType { Flicker, Change, Candle };

public class Lights : MonoBehaviour
{
	[SerializeField] public ActionType type;
	public GameObject objectToDestroy;
	[Header("Flicker")]
	[SerializeField] public GameObject lightObject;
	public float maxWait = 1;
	public float maxFlicker = 0.2f;
	public int countOfFlickers = 2;
	public float minIntensity = 0.5f;
	[Header("Change")]
	[SerializeField] public GameObject lightOff;
	[SerializeField] public GameObject lightOn;
	public float waitTime = 2;
	[Header("Candle")]
	[SerializeField] public GameObject candleLightObject;
	[SerializeField] public float frequency;

	private bool safeModeOn;
	private Light2D flickerlight;
	private Light2D candlelight;

	private void Start()
	{
		//safeModeOn = PlayerPrefs.GetInt("SafeModeOn", 0) == 1;
		if(type == ActionType.Candle)
		{
			candlelight = candleLightObject.GetComponent<Light2D>();
			StartCoroutine(candle());
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name == "Player")
		{
			switch (type)
			{
				case ActionType.Change:
					{
						StartCoroutine(changeLight());
						
						break;
					}
				case ActionType.Flicker:
					{
						StartCoroutine(flickerLight());
						break;

					}
			}
		}
	}

	IEnumerator flickerLight(bool leaveOn = true)
	{
		if(!safeModeOn)
		{
			flickerlight = lightObject.GetComponent<Light2D>();
			flickerlight.intensity = minIntensity;
			yield return new WaitForSeconds(Random.Range(0, maxFlicker));
			for (int i = 0; i < countOfFlickers; i++)
			{
				flickerlight.intensity = 0.9f;
				yield return new WaitForSeconds(Random.Range(0,maxWait));
				flickerlight.intensity = minIntensity;
				yield return new WaitForSeconds(Random.Range(0, maxFlicker));
			}
			if(leaveOn)
				flickerlight.intensity = 0.9f;
			if(type  == ActionType.Flicker)
				Destroy(objectToDestroy);
		}
	}

	IEnumerator changeLight()
	{
		StartCoroutine(flickerLight(false));
		yield return new WaitForSeconds(waitTime);
		Light2D light1 = lightOff.GetComponent<Light2D>();
		Light2D light2 = lightOn.GetComponent<Light2D>();
		light1.intensity = 0;
		light2.intensity = 0.9f;
		Destroy(objectToDestroy);
	}

	IEnumerator candle()
	{
		while(true) {
			candlelight.intensity = Random.Range(0.8f, 1.2f);
			yield return new WaitForSeconds(frequency);
		}

	}
}
