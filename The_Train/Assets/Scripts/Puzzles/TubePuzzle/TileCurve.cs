
public class TileCurve : Tile
{
    public override void StartFilling()
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
        StartCoroutine(FillThis(_fillFrames));
    }
    public override bool CheckInputDirectionValid(Direction input)
    {
        if (inputDir == input) return true;
        else if (outputDir == input)
        {
            Direction toSwap = inputDir;
            inputDir = outputDir;
            outputDir = toSwap;
            return true;
        }
        else
        {
            return false;
        }
    }
}
