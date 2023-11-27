using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject dialogueObject;
    public bool destroyObjectAfter;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (dialogueObject != null)
        {
            TriggerDialogue();
            if(destroyObjectAfter)
            {
                Destroy(gameObject);
            }
        }
	}
}
