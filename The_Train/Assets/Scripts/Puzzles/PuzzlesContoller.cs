using UnityEngine;

public class PuzzlesContoller : MonoBehaviour
{
    //[SerializeField] PuzzleBase _currentPuzzle; хз нужно ли вообще это поле
    [SerializeField] GameObject _supportTextView;

    public static PuzzlesContoller instance;

    InteractiveZone currentInteraction;
    internal bool nearInteractionObject = false;
    internal bool isOpened;

    //support fields
    PlayerController pl;
    DialogueManager dm;
    PauseMenu pm;

    bool canPuzzleInteract = true;
    private void Awake()
    {
        instance = this;
        pl = FindObjectOfType<PlayerController>();
        dm = FindObjectOfType<DialogueManager>();
        pm = FindObjectOfType<PauseMenu>();
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
        if (nearInteractionObject && !dm.isOpened && !pm.isOpened && !isOpened && canPuzzleInteract)
        {
            isOpened = true;
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
    public void BlockInteraction()
    {
        canPuzzleInteract = false;
        if (isOpened) Loose();
        UpdateSupportInteractTextView(false);
    }
    public void Win()
    {
        isOpened = false;
        if(currentInteraction._winItem!=null) Inventory.instance.AddItem(currentInteraction._winItem._id);
        bool destroyed = currentInteraction.AfterUse();
        if (destroyed)
        {
            InteractWithObject();//reset last interaction if it destroyed
        }
        pl.canMove = true;
    }
    public void Loose()
    {
        isOpened = false;
        pl.canMove = true;
    }
}
