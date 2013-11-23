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
    /// 背景音效，为选中小方块时候的效果音
    /// </summary>
    public interface IBackgroundSound
    {
        /// <summary>
        /// 获取是否应该发出音效
        /// </summary>
        bool Sound { get; set; }

        /// <summary>
        /// 获取背景音效
        /// </summary>
        SoundPlayer BGSound { get; set; }

    }
}