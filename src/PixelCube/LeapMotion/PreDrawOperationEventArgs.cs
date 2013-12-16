using System;
using System.Windows.Media.Media3D;
using Leap;

namespace PixelCube.LeapMotion
{
    public class PreDrawOperationEventArgs : EventArgs
    {
        public Point3D DrawPosition { set; get; }

        public PreDrawOperationEventArgs(Point3D drawPosition)
        {
            DrawPosition = drawPosition;
        }
    }
}
