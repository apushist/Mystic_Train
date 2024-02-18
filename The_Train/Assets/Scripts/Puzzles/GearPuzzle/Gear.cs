using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField] public bool isTouchable = true;
    [SerializeField] internal bool isSetted;
    [SerializeField] internal bool rotationRight;
    [SerializeField] internal bool ableRotate;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float radiusMult;

    int rMultiplier = 1;
    RectTransform rt;
    public float radius;
    private void Start()
    {
        rt = GetComponent<RectTransform>();
        radius = (rt.sizeDelta.x) / 2 * rt.localScale.x;// * radiusMult;
    }
    public void UpdateRotationDir(bool r)
    {
        rotationRight = r;
        rMultiplier = rotationRight ? 1 : -1;
    }
    public void EnableRotation(bool en)
    {
        ableRotate = en;
    }
    private void FixedUpdate()
    {
        if (ableRotate)
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * rMultiplier));
        }
    }
}
