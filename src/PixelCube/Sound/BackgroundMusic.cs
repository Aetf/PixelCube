using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace PixelCube.Sound
{
    /// <summary>
    /// 背景音乐循环播放
    /// </summary>
    public class BackgroundMusic:EventArgs
    {
        public bool isOpen
        {set;get;}
        public void PlayMusic(bool open)
        {
            System.Media.SoundPlayer Mu = new System.Media.SoundPlayer("../../Sound/April.wav");
            open = isOpen;
            if (open == true)
                Mu.PlayLooping();
            else
                Mu.Stop();
        }
    }
}
