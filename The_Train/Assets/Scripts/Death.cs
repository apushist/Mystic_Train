using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Death : MonoBehaviour
{
    public static Death instance;

    private PlayerController _player;
    private Inventory _inventory;
    private PuzzlesContoller _puzzles;
    private PauseMenu _pause;
    private DialogueManager _dialog;

    private Animator _animator;
    private Volume _pp;
    private ColorAdjustments _satur;

    [SerializeField] private GameObject _deathScreen;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _pp = Camera.main.GetComponent<Volume>();
        _pp.profile.TryGet(out _satur);

        _deathScreen.SetActive(false);
        _player = FindObjectOfType<PlayerController>();
        _inventory = FindObjectOfType<Inventory>();
        _puzzles = FindObjectOfType<PuzzlesContoller>();
        _pause = FindObjectOfType<PauseMenu>();
        _dialog = FindObjectOfType<DialogueManager>();

        _animator = GetComponent<Animator>();   
    }

    public void OnDeathTrigger()
    {
        BlockAll();
        StartCoroutine(DropSaturation());
        Invoke("EnableDeathScreen", 3);/////
		
	}

    void EnableDeathScreen()
	{
		_deathScreen.SetActive(true);
		_animator.SetTrigger("death");
	}
    public void BlockAll()
    {
        _inventory.BlockInventory();
        _puzzles.BlockInteraction();
        _pause.BlockPauseMenu();
        _dialog.BlockDialogue();
        _player.canMove = false;
    }

    public void BackToMainMenu()
    {
        SceneLoader.SwitchToScene("Menu");
    }

    IEnumerator DropSaturation()
    {
        int iters = 10;
        float time = 1.5f;
        float maxValue = _satur.saturation.value;
        float minValue = -100;
        for (int i = 0; i < iters; i++)
        {
            _satur.saturation.value = minValue + (maxValue-minValue) / iters * (iters - i - 1);
            yield return new WaitForSeconds(time / iters);
		}
		_player.DeathMonster();
	}
}
