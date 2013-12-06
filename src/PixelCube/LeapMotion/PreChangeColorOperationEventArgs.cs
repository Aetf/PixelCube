using Leap;

namespace PixelCube.LeapMotion
{
    public class PreChangeColorOperationEventArgs
    {
        public Vector Position { set; get; }
        public PreChangeColorOperationEventArgs(Vector position)
        {
            Position = position;
        }
    }
}
