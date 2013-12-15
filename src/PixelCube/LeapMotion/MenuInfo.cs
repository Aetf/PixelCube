using System;
using System.Windows.Media.Media3D;
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
        public Point3D TracePosition { get; set; }
        public TraceMenuArgs(Point3D tracePosition)
        {
            TracePosition = tracePosition;
        }
    }
}
