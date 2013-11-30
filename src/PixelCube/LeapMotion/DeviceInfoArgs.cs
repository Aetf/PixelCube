using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelCube.LeapMotion
{
    public class DeviceInfoArgs:EventArgs
    {
        private bool isConnected = false;
        public DeviceInfoArgs() { }

    }
}
