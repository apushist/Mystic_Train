using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public GameObject dialogueObject;
	public bool destroyObjectAfter;
	public Dialogue dialogue;
    
   

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        TriggerDialogue();
        if(destroyObjectAfter)
        {
            Destroy(gameObject);
        }
	}
}
