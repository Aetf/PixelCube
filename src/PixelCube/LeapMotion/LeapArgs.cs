using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace PixelCube.LeapMotion
{
    class LeapArgs:EventArgs
    {
        public Frame frame { set; get; }
    }
}
