using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;

public class End : MonoBehaviour
{
    public static End instance;

    private PlayerController _player;
    private Inventory _inventory;
    private PuzzlesContoller _puzzles;
    private PauseMenu _pause;
    private DialogueManager _dialog;

    private Animator _animator;
    private Volume _pp;
    private ColorAdjustments _satur;
    private Vignette _vignette;
    private LensDistortion _lens;

    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _camMain;
    [SerializeField] private GameObject _camShake;
    [SerializeField] private int _effectTime = 100;
    [SerializeField] private int _loreItemCountThreshold;
	/// <summary>
	/// поле для хранения текстов концовки в зависимости от найденных предметов
	/// 0 - хорошая 
	/// 1 - плохая
	/// </summary>
	[SerializeField] private string[] endingTexts = new string[2];


	private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _pp = Camera.main.GetComponent<Volume>();
        _pp.profile.TryGet(out _satur);
        _pp.profile.TryGet(out _lens);
        _pp.profile.TryGet(out _vignette); 

        _deathScreen.SetActive(false);
        _player = FindObjectOfType<PlayerController>();
        _inventory = FindObjectOfType<Inventory>();
        _puzzles = FindObjectOfType<PuzzlesContoller>();
        _pause = FindObjectOfType<PauseMenu>();
        _dialog = FindObjectOfType<DialogueManager>();

        _animator = GetComponent<Animator>();

        _camShake.SetActive(false);
    }

    public void OnEndTrigger()
    {
        BlockAll();
        StartCoroutine(DropSaturation());
        _camMain.SetActive(false);
        _camShake.SetActive(true);

        Invoke("EnableDeathScreen", 2);/////
    }

    void EnableDeathScreen()
    {
        if(_player.loreItemsFound >= _loreItemCountThreshold)
        {
            _deathScreen.GetComponentInChildren<TextMeshProUGUI>().text = endingTexts[0];
        }
        else { 
            _deathScreen.GetComponentInChildren<TextMeshProUGUI>().text = endingTexts[1];

		}
		_deathScreen.SetActive(true);
        _animator.SetTrigger("end");
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
        int iters = _effectTime;
        float maxValueSatur = _satur.saturation.value;
        float minValueSatur = -100;
        float maxValueDistort = _lens.intensity.value;
        float minValueDistort = -1f;
        float maxValueVignette = _vignette.intensity.value;
        float minValueVignette = 1f;
        for (int i = 0; i < iters; i++)
        {
            _satur.saturation.value = minValueSatur + (maxValueSatur - minValueSatur) / iters * (iters - i - 1);
            _lens.intensity.value = minValueDistort + (maxValueDistort - minValueDistort) / iters * (iters - i - 1);
            _vignette.intensity.value = minValueVignette + (maxValueVignette - minValueVignette) / iters * (iters - i - 1);
            yield return new WaitForFixedUpdate();
        }
    }
}
