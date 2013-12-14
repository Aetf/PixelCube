using System;
using Leap;
namespace PixelCube.LeapMotion
{
    public class PreEraseOperationEventArgs : EventArgs
    {
        public Vector Position { set; get; }

        public PreEraseOperationEventArgs(Vector position)
        {
            Position = position;
        }
    

    }
}
