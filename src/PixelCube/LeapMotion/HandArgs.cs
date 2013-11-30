using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace PixelCube.LeapMotion
{
    public class HandArgs:EventArgs
    {
        public Hand hand { private set; get; }
        public HandArgs(Hand hand)
        {
            this.hand = hand;
        }
    }
}
