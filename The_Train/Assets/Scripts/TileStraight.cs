using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileStraight : Tile
{
    public override void StartInflate()
    {
        if (inputDir == Direction.right)
        {
            if (outputDir == Direction.up)
            {
                _tubeContent.fillOrigin = (int)Image.Origin90.TopRight;
                _tubeContent.fillClockwise = true;
            }
            else
            {
                _tubeContent.fillOrigin = (int)Image.Origin90.BottomRight;
                _tubeContent.fillClockwise = false;
            }
        }
        else if (inputDir == Direction.down)
        {
            if (outputDir == Direction.right)
            {
                _tubeContent.fillOrigin = (int)Image.Origin90.BottomRight;
                _tubeContent.fillClockwise = true;
            }
            else
            {
                _tubeContent.fillOrigin = (int)Image.Origin90.BottomLeft;
                _tubeContent.fillClockwise = false;
            }

        }
        else if (inputDir == Direction.left)
        {
            if (outputDir == Direction.down)
            {
                _tubeContent.fillOrigin = (int)Image.Origin90.BottomLeft;
                _tubeContent.fillClockwise = true;
            }
            else
            {
                _tubeContent.fillOrigin = (int)Image.Origin90.TopLeft;
                _tubeContent.fillClockwise = false;
            }

        }
        else
        {
            if (outputDir == Direction.left)
            {
                _tubeContent.fillOrigin = (int)Image.Origin90.TopLeft;
                _tubeContent.fillClockwise = true;
            }
            else
            {
                _tubeContent.fillOrigin = (int)Image.Origin90.TopRight;
                _tubeContent.fillClockwise = false;
            }

        }
        StartCoroutine(inflation(_inflationTime, _inflationFrames));
    }
}
