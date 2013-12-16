using System;
using Leap;
using System.Windows.Media.Media3D;

namespace PixelCube.LeapMotion
{
    public class PreChangeColorOperationEventArgs : EventArgs
    {
        public Point3D Position { set; get; }
        public PreChangeColorOperationEventArgs(Point3D position)
        {
            Position = position;
        }
    }
}
