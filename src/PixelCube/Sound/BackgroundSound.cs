using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace PixelCube.Sound
{
     /// <summary>
     /// 背景音设置
     /// </summary>
    public class BackgroundSound:EventArgs
    {
        public bool StatusChanged{set;get;}

        public void PlaySound(bool play)
        {
            play = StatusChanged;
            SoundPlayer Sound = new SoundPlayer("../../Sound/909.wav");
            if (play == true)
                Sound.Play();
            else
                Sound.Dispose();
        }
    }
}
