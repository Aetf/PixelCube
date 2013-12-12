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
    /// 代表一个作品
    /// </summary>
    public interface IArtwork
    {
        /// <summary>
        /// 背景颜色
        /// </summary>
        Color BackgroundFill { get; set; }

        /// <summary>
        /// 画布的大小,SceneSize.X\Y\Z表示每行的小方块数量
        /// </summary>
        Vector3D SceneSize { get; set; }

        /// <summary>
        /// 返回场景中所有的小方块，位于(i, j, k)的小方块的索引为 i+SceneSize.x*j+SceneSize.y*k
        /// </summary>
        List<ICube> Cubes { get; set; }

        /// <summary>
        /// 获取和设置对应的文件名
        /// </summary>
        String FileName { get; set; }
    }
}
