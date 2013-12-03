
namespace PixelCube.LeapMotion
{
    public class PreScaleOperationEventArgs
    {
        public float ScaleFactor { set; get; }

        public PreScaleOperationEventArgs(float scaleFactor)
        {
            ScaleFactor = scaleFactor;
        }
    }
}
