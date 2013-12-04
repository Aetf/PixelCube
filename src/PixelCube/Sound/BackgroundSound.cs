using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace PixelCube.Sound
{
    public class BackgroundSound
    {
        public bool StatusChanged;
        public bool GetStatus
        {
            get;
            set;


        }
        public void PlaySound(bool play)
        {
            play = StatusChanged;
            SoundPlayer Sound = new SoundPlayer("");
            if (play == true)
                Sound.Play();
            else
                Sound.Dispose();
        }
    }
}
