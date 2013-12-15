using System;
using Leap;

namespace PixelCube.LeapMotion
{
    public class ExhaleMenuArgs : EventArgs
    {
        public ExhaleMenuArgs()
        {
        }
    }

    public class SelectMenuArgs : EventArgs
    {
        public SelectMenuArgs()
        {
        }
    }

    public class TraceMenuArgs : EventArgs
    {
        public float TracePositionX { set; get; }
        public float TracePositionY { set; get; }
        public float TracePositionZ { set; get; }
        public TraceMenuArgs(Vector tracePosition)
        {
            TracePositionX = tracePosition.x;
            TracePositionY = tracePosition.y;
            TracePositionZ = tracePosition.z;
        }
    }
}
