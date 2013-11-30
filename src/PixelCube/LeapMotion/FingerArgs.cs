using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace PixelCube.LeapMotion
{
    public class FingerArgs:EventArgs
    {
        public Finger finger { private set; get; }
        public FingerArgs(Finger finger)
        {
            this.finger = finger;
        }
    }
}
