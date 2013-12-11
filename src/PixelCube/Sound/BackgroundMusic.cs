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
    public class BackgroundMusic
    {
        public BackgroundMusic(bool open)
        {
            isOpen = open;
        }
        public bool isOpen;
        public void PlayMusic()
        {
            System.Media.SoundPlayer Mu = new System.Media.SoundPlayer("../../Sound/etudeofwater.wav");
            //open = isOpen;
            if (isOpen == true)
                Mu.PlayLooping();
            else
                Mu.Stop();
        }
    }
}
