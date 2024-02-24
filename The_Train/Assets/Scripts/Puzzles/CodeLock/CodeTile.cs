using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodeTile : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _codeText;
    public void Init(int n)
    {
        _codeText.text = n.ToString();
    }
}
