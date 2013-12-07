using Leap;
namespace PixelCube.LeapMotion
{
    public class PreEraseOperationEventArgs
    {
        public Vector Position { set; get; }

        public PreEraseOperationEventArgs(Vector position)
        {
            Position = position;
        }
    

    }
}
