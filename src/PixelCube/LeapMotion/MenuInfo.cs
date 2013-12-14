using Leap;

namespace PixelCube.LeapMotion
{
    public class ExhaleMenuArgs
    {
        public ExhaleMenuArgs()
        {
        }
    }

    public class SelectMenuArgs
    {
        public SelectMenuArgs()
        {
        }
    }

    public class TraceMenuArgs
    {
        public Vector TracePosition { set; get; }
        public TraceMenuArgs(Vector tracePosition)
        {
            TracePosition = tracePosition;
        }
    }
}
