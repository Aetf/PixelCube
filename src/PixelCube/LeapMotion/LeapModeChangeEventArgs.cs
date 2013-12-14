using System;

namespace PixelCube.LeapMotion
{
    public enum State
    {
        Drawing,
        Normal,
        Erasing,
        ChangingColor,
        Menu
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
