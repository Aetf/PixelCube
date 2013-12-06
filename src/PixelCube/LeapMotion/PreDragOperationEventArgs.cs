using Leap;

namespace PixelCube.LeapMotion
{
    
    public class PreDragOperationEventArgs
    {
        public Vector TransVector { set; get; }
        public PreDragOperationEventArgs(Vector transVector)
        {
            TransVector = transVector;
        }
    }
}
