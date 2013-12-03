using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelCube.LeapMotion
{
    public class LeapStatusChangeEventArgs:EventArgs
    {
        public bool isConnected { set; get; }
        public LeapStatusChangeEventArgs() 
        {
            this.isConnected = false;
        }


    }
}
