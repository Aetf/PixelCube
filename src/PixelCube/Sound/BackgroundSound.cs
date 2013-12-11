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
     /// 背景音设置
     /// </summary>
    public class BackgroundSound
    {

        public BackgroundSound()
        {
            //opcore.PostFocusOperationEvent += opcore_PostFocusOperationEvent;
            //opcore.PostDrawOperationEvent += opcore_PostDrawOperationEvent; 
        }
        OpCore opcore = new OpCore();


        /// <summary>
        /// 焦点变化时音效处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FocusOperationSound(object sender, PostFocusOperationEventArgs e)
        {
            SoundPlayer Sound = new SoundPlayer("../../Sound/focussound.wav");

            if (PostFocusOperationEvent != null)
                Sound.Play();
            else
                Sound.Dispose();
        }

        /// <summary>
        /// 小方块着色时音效处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DrawOperationSound(object sender, PostDrawOperationEventArgs e)
        {
            SoundPlayer Sound = new SoundPlayer("../../Sound/drawsound.wav");
            if (PostDrawOperationEvent != null)
                Sound.Play();
            else
                Sound.Dispose();

        }
        /// <summary>
        /// 擦除小方块时音效处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EraseOperationSound(object sender, PostEraseOperationEventArgs e)
        {
            SoundPlayer Sound = new SoundPlayer("../../Sound/erasesound.wav");
            if (PostEraseOperationEvent != null)
                Sound.Play();
            else
                Sound.Dispose();

        }

        //void opcore_PostDrawOperationEvent(object sender, PostDrawOperationEventArgs e)
        //{
        //    SoundPlayer Sound=new SoundPlayer("../../Sound/drawsound.wav");
        //    Sound.Play();
        //    throw new NotImplementedException();
        //}
        //private void opcore_PostFocusOperationEvent(object sender, PostFocusOperationEventArgs e)
        //{
        //    SoundPlayer Sound=new SoundPlayer("../../Sound/focussound.wav");
        //    Sound.Play();
        //    throw new NotImplementedException();
        //}
    }
}
