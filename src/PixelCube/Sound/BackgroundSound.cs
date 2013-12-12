using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using PixelCube.LeapMotion;
using PixelCube.Operations;

namespace PixelCube.Sound
{
     /// <summary>
     /// 音效控制
     /// </summary>
    public class BackgroundSound
    {
        SoundPlayer focus = new SoundPlayer("../../Sound/focussound.wav");
        SoundPlayer erase = new SoundPlayer("../../Sound/erasesound.wav");
        SoundPlayer draw = new SoundPlayer("../../Sound/drawsound.wav");

        public void DoInit(MainWindow win)
        {
            focus.Load();
            draw.Load();
            erase.Load();
        }

        /// <summary>
        /// 焦点变化时音效处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FocusOperationSound(object sender, PostFocusOperationEventArgs e)
        {
            focus.Play();
        }

        /// <summary>
        /// 小方块着色时音效处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DrawOperationSound(object sender, PostDrawOperationEventArgs e)
        {
            draw.Play();
        }

        /// <summary>
        /// 擦除小方块时音效处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EraseOperationSound(object sender, PostEraseOperationEventArgs e)
        {
            erase.Play();
        }
    }
}
