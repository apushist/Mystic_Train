using System.Collections.Generic;
using UnityEngine;

public class GearPuzzle : PuzzleBase
{
    [SerializeField] private GameObject _puzzleScreen;
    [SerializeField] public RectTransform _parentSprite;
    [SerializeField] private GameObject _frontObs;
    [SerializeField] private bool _haveGearInHand = false;

    [SerializeField] private Gear _currentGear = null;

    [SerializeField] internal List<GearTarget> _gearList = new List<GearTarget>();

    [SerializeField] float _maxSetOffset;

    GearTarget startGear;
    GearTarget endGear;
    List<Vector2> _gearTargetPos = new List<Vector2>();
    public static GearPuzzle instance;
    

    private void Awake()
    {
        instance = this;
        
        foreach (GearTarget gear in _gearList)
        {
            _gearTargetPos.Add(gear.transform.parent.GetComponent<RectTransform>().anchoredPosition);
        }
        startGear = _gearList[0];
        endGear = _gearList[_gearList.Count-1];
        EnableThisPuzzle(false);
    }
    public override void StartPuzzle()
    {
        EnableThisPuzzle(true);
        _frontObs.SetActive(false);
        UpdateGearStatus();
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
        for (int k = 0; k < 2; k++)
        {
            bool[] _gearIndexToRotate = new bool[_gearTargetPos.Count];
            for (int i = 0; i < _gearTargetPos.Count; i++)
                _gearIndexToRotate[i] = false;
            for (int i = 0; i < _gearTargetPos.Count; i++)
            {
                for (int j = i + 1; j < _gearTargetPos.Count; j++)
                {
                    if (_gearList[i].gear == null || _gearList[j].gear == null)
                        continue;
                    var magn = (_gearTargetPos[j] - _gearTargetPos[i]).magnitude;
                    if (magn < Mathf.Abs(_gearList[i].gear.radius + _gearList[j].gear.radius) && magn > Mathf.Abs(_gearList[i].gear.radius + _gearList[j].gear.radius - _maxSetOffset))
                    {
                        if (_gearList[i].gear.GetEnableRotation())
                        {
                            _gearList[j].gear.SetEnableRotation(true);
                            _gearList[j].gear.UpdateRotationDir(!_gearList[i].gear.rotationRight);
                            _gearIndexToRotate[j] = true;
                        }
                        else
                        {
                            _gearList[j].gear.SetEnableRotation(false);
                            _gearIndexToRotate[j] = false;
                        }
                    }
                }
            }
            for (int i = 1; i < _gearTargetPos.Count; i++)
            {
                if (!_gearIndexToRotate[i] && _gearList[i].gear != null)
                    _gearList[i].gear.SetEnableRotation(false);
            }
        }
        CheckEndGear();
    }
    public void CheckEndGear()
    {
        if (endGear.gear.GetEnableRotation())
        {
            _frontObs.SetActive(true);
            Invoke("WinPuzzle", 1);
            //WinPuzzle();
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
