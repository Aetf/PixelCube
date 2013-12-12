using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelCube.LeapMotion
{
    public enum State
    {
        drawing,
        focusing,
        erasing,
        colorChanging,
        menuSelecting
    }
    
    
    public class LeapModeChangeEventArgs:EventArgs
    {
        public State state { set; get; }


        public LeapModeChangeEventArgs(State state) 
        {
            this.state = state;
        }


    }
}
