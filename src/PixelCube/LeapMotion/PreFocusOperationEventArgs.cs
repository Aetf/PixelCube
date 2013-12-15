using System;
using Leap;

namespace PixelCube.LeapMotion
{
    public class PreFocusOperationEventArgs : EventArgs
    {
        public Vector FocusPosition { set; get; }

        public PreFocusOperationEventArgs(Vector focusPosition)
        {
            FocusPosition = focusPosition;
        }
    }
}
