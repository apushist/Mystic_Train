using UnityEngine;

public enum DoorType { Door,Gate };

public class Door : MonoBehaviour
{
	public DoorType type;
	public bool isLocked;
	[Header("Door")]
	public GameObject doorManager;
	[Header("Gate")]
	public GameObject objectToDestroy;
	private Animator anim;
	private AudioSource audioSource;

	// Start is called before the first frame update
	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		switch (type)
		{
			case DoorType.Door: {
					anim = doorManager.GetComponent<Animator>();
					break;
				}
			case DoorType.Gate:
				{
					anim = GetComponentInChildren<Animator>();
					break ;
				}
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isLocked)
		{
			switch (type)
			{
				case DoorType.Door: { anim.SetBool("IsPlayerNear", true); break; }
				case DoorType.Gate: {
					anim.SetTrigger("Open");
					Destroy(objectToDestroy);
					break;
				}
			}
			
		}
	}


	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!isLocked && type == DoorType.Door)
		{
			anim.SetBool("IsPlayerNear", false);
		}
	}
	public void SetDoorLock(bool isOn)
    {
		isLocked = isOn;
		if(audioSource!=null) audioSource.PlayOneShot(audioSource.clip);
		GetComponent<Collider2D>().enabled = false;
		GetComponent<Collider2D>().enabled = true;
	}
}
