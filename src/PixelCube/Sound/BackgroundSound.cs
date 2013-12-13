using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using PixelCube.LeapMotion;
using PixelCube.Operations;
using WMPLib;
using System.IO;
using System.Reflection;

namespace PixelCube.Sound
{
     /// <summary>
     /// 音效控制
     /// </summary>
    public class BackgroundSound
    {
        WindowsMediaPlayer focussound = new WindowsMediaPlayer();
        WindowsMediaPlayer erasesound = new WindowsMediaPlayer();
        WindowsMediaPlayer drawsound = new WindowsMediaPlayer();
        /// <summary>
        /// PixelCube path
        /// </summary>
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

        /// <summary>
        /// WMP 参数设置
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="name"></param>
        private void Load(WindowsMediaPlayer sound,string name)
        {
            sound.uiMode = "Invisible";
            sound.settings.autoStart = false;
            sound.URL = Path.Combine(Path.GetDirectoryName(SetPath()), "Sound//"+name+".wav");
            sound.ToString();
        }

        public void DoInit(MainWindow win)
        {
            Load(focussound,"focussound");
            Load(drawsound,"drawsound");
            Load(erasesound,"erasesound");
        }

        /// <summary>
        /// 焦点变化时音效处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FocusOperationSound(object sender, PostFocusOperationEventArgs e)
        {
            focussound.controls.play();
        }

        /// <summary>
        /// 小方块着色时音效处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DrawOperationSound(object sender, PostDrawOperationEventArgs e)
        {
            drawsound.controls.play();
        }

        /// <summary>
        /// 擦除小方块时音效处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EraseOperationSound(object sender, PostEraseOperationEventArgs e)
        {
            erasesound.controls.play();
        }
    }
}
