using System;
using Leap;
using System.Windows.Media.Media3D;

namespace PixelCube.LeapMotion
{
    public class PreFocusOperationEventArgs : EventArgs
    {
        public Point3D FocusPosition { set; get; }

        public PreFocusOperationEventArgs(Point3D focusPosition)
        {
            FocusPosition = focusPosition;
        }
    }
}
