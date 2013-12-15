using System;
using Leap;

namespace PixelCube.LeapMotion
{

    public class PreDragOperationEventArgs : EventArgs
    {
        public Vector TransVector { set; get; }
        public PreDragOperationEventArgs(Vector transVector)
        {
            TransVector = transVector;
        }
    }
}
