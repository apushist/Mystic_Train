using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public GameObject dialogueObject;
	public bool destroyObjectAfter;
	public Dialogue dialogue;
    public bool partOfLore = false;
    
   

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (partOfLore)
		{
			FindObjectOfType<PlayerController>().loreItemsFound++;
		}
		TriggerDialogue();
        if(destroyObjectAfter)
        {
            Destroy(gameObject);
        }
	}
}
