using System;
using Leap;
using System.Windows.Media.Media3D;
namespace PixelCube.LeapMotion
{
    public class PreEraseOperationEventArgs : EventArgs
    {
        public Point3D Position { set; get; }

        public PreEraseOperationEventArgs(Point3D position)
        {
            Position = position;
        }
    

    }
}
