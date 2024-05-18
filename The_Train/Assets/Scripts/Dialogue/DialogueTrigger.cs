using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public GameObject dialogueObject;
	public bool destroyObjectAfter;
	public Dialogue dialogue;
	public GameObject animObject;

	private Animator animator;

	private void Start()
	{
		if(animObject != null)
			animator = animObject.GetComponent<Animator>();
	}

	public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue,animator);
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
