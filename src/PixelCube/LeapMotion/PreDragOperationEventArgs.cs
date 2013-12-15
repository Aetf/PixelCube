using System;
using Leap;
using System.Windows.Media.Media3D;

namespace PixelCube.LeapMotion
{

    public class PreDragOperationEventArgs : EventArgs
    {
        public Vector3D TransVector { set; get; }
        public PreDragOperationEventArgs(Vector3D transVector)
        {
            TransVector = transVector;
        }
    }
}
