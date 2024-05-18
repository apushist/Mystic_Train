using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public GameObject dialogueObject;
	public bool destroyObjectAfter;
	public Dialogue dialogue;
	public Animator attachedObjectToAnimate;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue,attachedObjectToAnimate);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			
			TriggerDialogue();
			if (destroyObjectAfter)
			{
				Destroy(gameObject);
			}
		}
	}
	
}
