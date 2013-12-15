using System;

namespace PixelCube.LeapMotion
{
    public class LeapConnectionChangedEventArgs : EventArgs
    {
        public bool Connected { get; set; }
    }
}
