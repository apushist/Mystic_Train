using UnityEngine;
using UnityEngine.UI;

public class ColorPanel : MonoBehaviour
{
    [SerializeField] public PanelRotateState _state;
    [SerializeField] public Transform _itemsParent;
    [HideInInspector] public int[] _itemsIndex;
    [HideInInspector] public RectTransform[] _itemsObject;

    void Start()
    {
        int iters = _itemsParent.childCount;
        _itemsIndex = new int[iters];
        _itemsObject = new RectTransform[iters];
        for (int i = 0; i < iters; i++)
        {
            _itemsObject[i] = _itemsParent.GetChild(i).GetComponent<RectTransform>();
            for (int j = 0; j < iters; j++)
            {
                if (ColorPanelsPuzzle.instance._itemsReference[j] == _itemsObject[i].GetComponent<Image>().sprite)
                {
                    _itemsIndex[i] = j;
                }
            }
        }
        
    }
    internal void NextState()
    {
        if (_state == PanelRotateState.left)
            _state = PanelRotateState.up;
        else
            _state++;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
public enum PanelRotateState { up, right, down, left }
