using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField] public bool isTouchable = true;
    internal bool isSetted;
    [SerializeField] internal bool rotationRight;
    [SerializeField] private bool ableRotate;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float radiusMult;

    int rotateMult = 1;
    RectTransform rt;
    internal float radius;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        radius = (rt.sizeDelta.x) / 2 * rt.localScale.x * radiusMult;
        UpdateRotationDir(rotationRight);
    }
    public void UpdateRotationDir(bool r)
    {
        rotationRight = r;
        rotateMult = rotationRight ? 1 : -1;
    }
    public void SetEnableRotation(bool en)
    {
        ableRotate = en;
    }
    public bool GetEnableRotation()
    {
        return ableRotate;
    }
    private void FixedUpdate()
    {
        if (ableRotate)
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * rotateMult));
        }
    }
}
