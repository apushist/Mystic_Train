using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTarget : MonoBehaviour
{
    [SerializeField] public Gear gear;

    private void Start()
    {
        if (gear != null && !gear.isSetted)
        {
            gear.isSetted = true;
            SetGearPositionToThis();
        }
        else
        {
            gear = null;
        }
    }
    public void ClickGear()
    {
        if (GearPuzzle.instance.HaveGearInHand() && gear == null)
        {
            SetGearToThis();
        }
        else if (!GearPuzzle.instance.HaveGearInHand() && gear != null && gear.isTouchable)
        {
            GetGearOfThis();
        }
        GearPuzzle.instance.UpdateGearStatus();
    }
    public void SetGearToThis()
    {
        gear = Instantiate(GearPuzzle.instance.GetCurrentGear(), GearPuzzle.instance._parentSprite);
        SetGearPositionToThis();
        GearPuzzle.instance.DestroyGearInHand();
    }
    public void SetGearPositionToThis()
    {
        gear.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
    }
    public void GetGearOfThis()
    {
        gear.SetEnableRotation(false);
        GearPuzzle.instance.SetGearToHand(gear);
        Destroy(gear.gameObject);
        gear = null;
    }
}
