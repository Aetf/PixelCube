using Leap;

namespace PixelCube.LeapMotion
{
    class PreDrawOperationEventArgs
    {
        public Vector DrawPosition { set; get; }

        public PreDrawOperationEventArgs(Vector drawPosition)
        {
            DrawPosition = drawPosition;
        }
    }
}
