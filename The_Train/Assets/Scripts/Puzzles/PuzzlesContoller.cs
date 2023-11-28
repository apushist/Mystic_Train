using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlesContoller : MonoBehaviour
{
    [SerializeField] PuzzleBase _currentPuzzle;
    [SerializeField] GameObject _supportTextView;

    public static PuzzlesContoller instance;

    InteractiveZone currentInteraction;
    PlayerController pl;
    internal bool nearInteractionObject = false;

    private void Awake()
    {
        instance = this;
        pl = FindObjectOfType<PlayerController>();
        UpdateSupportInteractTextView(false);
        PlayerController.Epressed += StartPuzzleLogic;
    }

    public void InteractWithObject(InteractiveZone col = null)
    {
        if (col != null)
        {
            currentInteraction = col;
            nearInteractionObject = true;
            UpdateSupportInteractTextView(true);
        }
        else
        {
            currentInteraction = null;
            nearInteractionObject = false;
            UpdateSupportInteractTextView(false);
        }
    }
    public void StartPuzzleLogic()
    {
        if (nearInteractionObject)
        {            
            pl.canMove = false;
            currentInteraction._puzzle.StartPuzzle();
        }
    }
    private void Update()
    {
        if (nearInteractionObject && Input.GetKeyDown(KeyCode.Q))
        {
            currentInteraction._puzzle.StartPuzzle();
        }
    }
    void UpdateSupportInteractTextView(bool enabl)
    {
        _supportTextView.SetActive(enabl);
    }
    public void Win()
    {
        Debug.Log("win 1");
        Inventory.instance.AddItem(currentInteraction._winItem._id);
        bool destroyed = currentInteraction.AfterUse();
        if (destroyed)
        {
            InteractWithObject();//reset last interaction if it destroyed
        }
        pl.canMove = true;
    }
    public void Loose()
    {
        Debug.Log("loose 1");
        pl.canMove = true;
    }
}
