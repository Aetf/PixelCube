using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelCube.LeapMotion
{
    public class DeviceInfoArgs:EventArgs
    {
        public bool isConnected { set; get; }
        public DeviceInfoArgs() 
        {
            this.isConnected = false;
        }


    }
}
