using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileCurve : Tile
{
    public override void StartInflate()
    {
        if (inputDir == Direction.right && outputDir == Direction.down ||
           inputDir == Direction.down && outputDir == Direction.left ||
           inputDir == Direction.left && outputDir == Direction.up ||
           inputDir == Direction.up && outputDir == Direction.right)
        {
            _tubeContent.fillClockwise = false;
        }
        else
        {
            _tubeContent.fillClockwise = true;
        }
        StartCoroutine(inflation(_inflationTime, _inflationFrames));
    }
}
