using System;

namespace PixelCube.LeapMotion
{
    public class PreScaleOperationEventArgs : EventArgs
    {
        public float ScaleFactor { set; get; }

        public PreScaleOperationEventArgs(float scaleFactor)
        {
            ScaleFactor = scaleFactor;
        }
    }
}
