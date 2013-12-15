using System;
using Leap;

namespace PixelCube.LeapMotion
{
    public class PreChangeColorOperationEventArgs : EventArgs
    {
        public Vector Position { set; get; }
        public PreChangeColorOperationEventArgs(Vector position)
        {
            Position = position;
        }
    }
}
