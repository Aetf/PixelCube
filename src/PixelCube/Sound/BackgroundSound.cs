using System.IO;
using System.Reflection;
using PixelCube.Operations;
using WMPLib;

namespace PixelCube.Sound
{
     /// <summary>
     /// 音效控制
     /// </summary>
    public class BackgroundSound
    {
        static WindowsMediaPlayer focussound = new WindowsMediaPlayer();
        static WindowsMediaPlayer erasesound = new WindowsMediaPlayer();
        static WindowsMediaPlayer drawsound = new WindowsMediaPlayer();

        /// <summary>
        /// WMP 参数设置
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="name"></param>
        private static void Load(WindowsMediaPlayer sound,string name)
        {
            sound.uiMode = "Invisible";
            sound.settings.autoStart = false;
            sound.URL = "res//"+name+".wav";
        }

        static BackgroundSound()
        {
            Load(focussound, "focussound");
            Load(drawsound, "drawsound");
            Load(erasesound, "erasesound");
        }

        public void DoInit(MainWindow win)
        {
            win.kernel.PostDrawOperationEvent += DrawOperationSound;
            win.kernel.PostFocusOperationEvent += FocusOperationSound;
            win.kernel.PostEraseOperationEvent += EraseOperationSound;
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
