using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Media;

namespace PixelCube.Interfaces
{
    /// <summary>
    /// 背景音乐
    /// </summary>
    public interface IBackgroundMusic
    {
        /// <summary>
        /// 设置背景音乐开关
        /// </summary>
        bool IsOpen { get; set; }

        /// <summary>
        /// 获取背景音乐
        /// </summary>
        SoundPlayer BGMusic { get; set; }
    }
}