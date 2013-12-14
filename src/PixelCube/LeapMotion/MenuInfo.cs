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
        public Vector TracePosition { set; get; }
        public TraceMenuArgs(Vector tracePosition)
        {
            TracePosition = tracePosition;
        }
    }
}
