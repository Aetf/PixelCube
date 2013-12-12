using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using PixelCube.Interfaces;
using System.ComponentModel;

namespace PixelCube.Sound
{
    /// <summary>
    /// 背景音乐循环播放
    /// </summary>
    public class BackgroundMusic : IVolume
    {
        SoundPlayer mu = new System.Media.SoundPlayer("../../Sound/etudeofwater.wav");

        public void DoInit(MainWindow win, bool open)
        {
            mu.Load();
            Mute = open;
        }

        private bool mute;
        public bool Mute
        {
            get
            {
                return mute;
            }
            set
            {
                mute = value;
                if (!value)
                {
                    mu.Stop();
                }
                else
                {
                    mu.PlayLooping();
                }
            }
        }
    }
}
