using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PixelCube.LeapMotion
{
    public interface ILeapMotion
    {
        public event EventHandler<EventArgs> LeapStatusInfom;
        public event EventHandler<LeapMotion.FingerArgs> Dye;
        public event EventHandler<LeapMotion.FingerArgs> Move;
        public event EventHandler<EventArgs> Rotate;
        public event EventHandler<EventArgs> Zoom;
    }
}
