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
    /// 表示一个场景控制器
    /// </summary>
    public interface ISceneControler
    {
        /// <summary>
        /// 获取和设置对于整个世界坐标的变换
        /// </summary>
        MatrixTransform3D WorldTransform { get; set; }
    }
}
