using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GearPuzzle : PuzzleBase
{
    [SerializeField] private GameObject _puzzleScreen;
    [SerializeField] public RectTransform _parentSprite;//
    [SerializeField] private bool _haveGearInHand = false;

    [SerializeField] private Gear _currentGear = null;
    internal List<GearTarget> _gearList = new List<GearTarget>();
    public static GearPuzzle instance;

    private void Awake()
    {
        instance = this;
        //EnableThisPuzzle(false);
    }
    public override void StartPuzzle()
    {
        EnableThisPuzzle(true);
    }

    private void EnableThisPuzzle(bool isOn)
    {
        _puzzleScreen.SetActive(isOn);
    }

    public bool HaveGearInHand()
    {
        return _haveGearInHand;
    }
    public void SetGearToHand(Gear gear)
    {
        _haveGearInHand = true;
        _currentGear = Instantiate(gear, _parentSprite);
    }
    public void DestroyGearInHand()
    {
        _haveGearInHand = false;
        Destroy(_currentGear.gameObject);
    }
    public Gear GetCurrentGear()
    {
        return _currentGear;
  
    }
    private void Update()
    {
        if(_haveGearInHand)
        {
            _currentGear.GetComponent<RectTransform>().position = Input.mousePosition;// + new Vector3(10, -10, 0);
        }
    }
    public void UpdateGearStatus()
    {
        List<Vector2> list = new List<Vector2>();
        foreach(GearTarget gear in _gearList)
        {
            list.Add(gear.transform.parent.GetComponent<RectTransform>().anchoredPosition);
        }
        for(int i = 0; i < list.Count; i++)
        {
            for(int j = i+1; j < list.Count; j++)
            {
                if (_gearList[i].gear == null || _gearList[j].gear == null)
                    continue;
                var magn = (list[j] - list[i]).magnitude;
                if (magn < Mathf.Abs(_gearList[i].gear.radius + _gearList[j].gear.radius))
                {
                    _gearList[i].gear.EnableRotation(true);
                    _gearList[j].gear.EnableRotation(true);
                }
                Debug.Log(magn + ", I: " + i + ", J: " + j + "::" + Mathf.Abs(_gearList[i].gear.radius + _gearList[j].gear.radius));
            }
        }

    }
    void ResetFillProgress()
    {
     
    }
    public override void WinPuzzle()
    {
        ResetFillProgress();
        EnableThisPuzzle(false);
        PuzzlesContoller.instance.Win();
    }
    public override void LoosePuzzle()
    {
        ResetFillProgress();
        EnableThisPuzzle(false);
        PuzzlesContoller.instance.Loose();
    }

}
