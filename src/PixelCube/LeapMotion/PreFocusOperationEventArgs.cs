using Leap;

namespace PixelCube.LeapMotion
{
   public class PreFocusOperationEventArgs
    {
        public Vector FocusPosition { set; get; }

        public PreFocusOperationEventArgs(Vector focusPosition)
        {
            FocusPosition = focusPosition;
        }
    }
}
