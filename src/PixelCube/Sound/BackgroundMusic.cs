using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using PixelCube.Interfaces;
using System.ComponentModel;
using WMPLib;
using System.IO;
using System.Reflection;

namespace PixelCube.Sound
{
    /// <summary>
    /// 背景音乐循环播放
    /// </summary>
    public class BackgroundMusic : IVolume
    {
        WMPLib.WindowsMediaPlayer BGM = new WindowsMediaPlayer();
        private string path;
        /// <summary>
        /// 获取音频文件的绝对路径
        /// </summary>
        public string SetPath()
        {
            DirectoryInfo dr = new DirectoryInfo(Assembly.GetEntryAssembly().Location);
            dr = dr.Parent.Parent;
            path = dr.FullName.ToString();
            return path;
        }
        public void DoInit(MainWindow win, bool open)
        {
            BGM.settings.setMode("loop", true);
            BGM.uiMode = "Invisible";
            BGM.URL = Path.Combine(Path.GetDirectoryName(SetPath()), "Sound//etudeofwater.wav");
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
