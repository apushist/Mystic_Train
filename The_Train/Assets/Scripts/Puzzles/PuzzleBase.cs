using UnityEngine;

public class PuzzleBase : MonoBehaviour
{
    public virtual void StartPuzzle() { }
    public virtual void WinPuzzle() { }
    public virtual void LoosePuzzle() { }
}
