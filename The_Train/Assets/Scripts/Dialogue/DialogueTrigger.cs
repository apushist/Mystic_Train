using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public GameObject dialogueObject;
	public bool destroyObjectAfter;
	public Dialogue dialogue;
    public bool partOfLore = false;
	public bool destroyObjectAfterAnimation;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (partOfLore)
			{
				FindObjectOfType<PlayerController>().loreItemsFound++;
			}
			TriggerDialogue();
			if (destroyObjectAfter)
			{
				Destroy(gameObject);
			}
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (destroyObjectAfterAnimation)
		{
			animator.SetTrigger("Play");
			Destroy(gameObject, 1f);

		}
	}
}
