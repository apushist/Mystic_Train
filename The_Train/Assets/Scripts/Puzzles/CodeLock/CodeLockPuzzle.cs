using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodeLockPuzzle : PuzzleBase
{
    [SerializeField] private int[] _lockPassword;
    
    public static CodeLockPuzzle instance;

    [Header("CodeSettings")]
    [SerializeField] private GameObject _puzzleScreen;
    [SerializeField] private Transform[] _codeLines;
    [SerializeField] private Transform[] _interactiveField;
    [SerializeField] private int[] _currentCodeStates;
    [SerializeField] private CodeTile _basicCodeTextPrefab;
    [SerializeField] private float _codeTextHorizontalOffset;
    [SerializeField] private float _codeTextSize;
    [SerializeField] private float _codeMoveTime;
    [SerializeField] private RectTransform _leftRightTarget;
    private int _codeTextCount = 10;

    bool inMove = false;


    private void Awake()
    {
        instance = this;        
        GenerateGrid();
        StartPuzzle();
        EnableThisPuzzle(false);
    }
    public override void StartPuzzle()
    {
        EnableThisPuzzle(true);
    }

    private void EnableThisPuzzle(bool isOn)
    {
        _puzzleScreen.SetActive(isOn);
    }
    public void GenerateGrid()
    {
        for (int x = 0; x < _codeLines.Length; x++)
        {
            for (int y = 0; y < _codeTextCount*2; y++)
            {
                var spawnedCodeTile = Instantiate(_basicCodeTextPrefab, _codeLines[x]);
                spawnedCodeTile.name = $"Tile {x} {y}";
                spawnedCodeTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(y * (_codeTextSize + _codeTextHorizontalOffset), 0);
                spawnedCodeTile.Init(y % _codeTextCount);
            }

        }
        if (_currentCodeStates == null)
        {
            _currentCodeStates = new int[4] { 10, 10, 10, 10 };
        }
        for (int i = 0; i < _codeLines.Length; i++)
        {
            SetCodeState(i, _currentCodeStates[i]);
            DisableInteractiveField(i);
        }

    }
    public void EnableInteractiveField(int i)
    {
        _interactiveField[i].gameObject.SetActive(true);
    }
    public void DisableInteractiveField(int i)
    {
        _interactiveField[i].gameObject.SetActive(false);
    }
    public void SetCodeState(int line, int state)
    {
        if(state > 15)
            state -= 10;
        else if(state < 5)
            state += 10;
        _currentCodeStates[line] = state;
        _codeLines[line].GetComponent<RectTransform>().anchoredPosition = new Vector2(-state * (_codeTextSize + _codeTextHorizontalOffset), 0);

        if (CheckPassword())
        {
            WinPuzzle();
        }
    }
    public void PressButtonLine(int line)
    {
        if (!inMove)
        {
            if (Input.mousePosition.x < _leftRightTarget.position.x)
            {
                StartCoroutine(moveCodeTile(_currentCodeStates[line], _currentCodeStates[line] - 1, line, false));
            }
            else
            {
                StartCoroutine(moveCodeTile(_currentCodeStates[line], _currentCodeStates[line] + 1, line, true));
            }
        }
    }
    IEnumerator moveCodeTile(int stateStart, int stateEnd, int line, bool right)
    {
        inMove = true;
        RectTransform rect = _codeLines[line].GetComponent<RectTransform>();
        for (int i = 0; i < 30; i++)
        {
            rect.anchoredPosition = new Vector2(-(stateStart + (float)(stateEnd - stateStart) * i / 30) * (_codeTextSize + _codeTextHorizontalOffset), 0);
            yield return new WaitForSeconds(_codeMoveTime / 30);
        }
        if (right)
            SetCodeState(line, _currentCodeStates[line] + 1);
        else
            SetCodeState(line, _currentCodeStates[line] - 1);
        inMove = false;        
    }
    bool CheckPassword()
    {
        for(int i = 0; i < 4; ++i)
        {
            if (_lockPassword[i] != _currentCodeStates[i] % 10)
            {
                return false;
            }
        }
        return true;
    }
    public void BackButton()
    {
        LoosePuzzle();
    }
    public override void WinPuzzle()
    {
        EnableThisPuzzle(false);
        PuzzlesContoller.instance.Win();
    }
    public override void LoosePuzzle()
    {
        EnableThisPuzzle(false);
        PuzzlesContoller.instance.Loose();
    }

    
}
