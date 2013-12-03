using Leap;

namespace PixelCube.LeapMotion
{
    public class PreDrawOperationEventArgs
    {
        public Vector DrawPosition { set; get; }

        public PreDrawOperationEventArgs(Vector drawPosition)
        {
            DrawPosition = drawPosition;
        }
    }
}
