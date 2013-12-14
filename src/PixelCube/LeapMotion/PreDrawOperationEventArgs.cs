using System;
using Leap;

namespace PixelCube.LeapMotion
{
    public class PreDrawOperationEventArgs : EventArgs
    {
        public Vector DrawPosition { set; get; }

        public PreDrawOperationEventArgs(Vector drawPosition)
        {
            DrawPosition = drawPosition;
        }
    }
}
