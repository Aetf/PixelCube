using System;
using Leap;

namespace PixelCube.LeapMotion
{
    public class PreRotateOperationEventArgs : EventArgs
    {
       public float RotationAngle;
       public Vector RotationAxis;
       
        public PreRotateOperationEventArgs(Vector rotationAxis, float rotationAngle)
        {
            RotationAngle = rotationAngle;
            RotationAxis = rotationAxis;
        }
    }
}
