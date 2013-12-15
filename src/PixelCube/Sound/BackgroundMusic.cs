using System.IO;
using System.Reflection;
using PixelCube.Interfaces;
using WMPLib;

namespace PixelCube.Sound
{
    /// <summary>
    /// 背景音乐循环播放
    /// </summary>
    public class BackgroundMusic : IVolume
    {
        WMPLib.WindowsMediaPlayer BGM = new WindowsMediaPlayer();
        private string path;

        public void DoInit(MainWindow win, bool open)
        {
            BGM.settings.setMode("loop", true);
            BGM.uiMode = "Invisible";
            BGM.URL = "res//etudeofwater.wav";
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
                    BGM.controls.stop();
                }
                else
                {
                    BGM.controls.play();
                }
            }
        }
    }
}
