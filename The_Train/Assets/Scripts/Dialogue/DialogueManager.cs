using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dialogueText;
    public Animator animator;
    public PlayerController playerController;
    internal bool isOpened;
    bool canUseDialogue = true;

	private Queue<string> sentences;
    private Animator animatorOfAttachedItem;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

	void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter)) && isOpened == true && canUseDialogue)
        {
            DisplayNextSentence();
        }
    }


	public void StartDialogue(Dialogue dialogue, Animator attachedAnimator = null)
    {
        if (!canUseDialogue) return;
        if(attachedAnimator != null)
        {
            animatorOfAttachedItem = attachedAnimator;
        }
        isOpened = true;
		playerController.canMove = false;
		animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;

        sentences.Clear();
		if (dialogue.partOfLore)
		{
			playerController.loreItemsFound++;
		}

		foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence)
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
	{
        isOpened = false;
        playerController.canMove = true;
		animator.SetBool("IsOpen", false);
        if(animatorOfAttachedItem != null)
        {
            animatorOfAttachedItem.SetTrigger("Play");
        }
    }

    public void BlockDialogue()
    {
        canUseDialogue = false;
        EndDialogue();
    }

}
