using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace PixelCube.Scene3D
{
    /// <summary>
    /// 表示存储一个小方块数据的类
    /// </summary>
    public interface ICube
    {
        /// <summary>
        /// 获取和设置小方块的颜色
        /// </summary>
        Color CubeColor { get; set; }

        /// <summary>
        /// 获取和设置小方块相对场景的位置。
        /// 比如在4x4x4的场景中，某个角即为(0, 0, 0)
        /// </summary>
        Vector3D Position { get; set; }


        /// <summary>
        /// 是否可见
        /// </summary>
         bool Visible{ get; set;}

        /// <summary>
        /// 是否拥有焦点
        /// </summary>
         bool Hasfocus{ get; set ; }

    }
}
