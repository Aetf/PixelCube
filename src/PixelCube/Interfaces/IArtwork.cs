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
        /// 画布的大小,SceneSize.Item1/Item2/Item3表示X/Y/Z方向上的小方块数量
        /// </summary>
        SceneSize3D SceneSize { get; set; }

        /// <summary>
        /// 返回场景中所有的小方块，位于(i, j, k)的小方块的索引为 i+SceneSize.Item1*j+SceneSize.Item1*SceneSize.Item2*k
        /// </summary>
        List<PixelCube.ThreeDimensional.Cube> Cubes { get; set; }

        /// <summary>
        /// 获取和设置对应的文件名
        /// </summary>
        String FileName { get; set; }
    }

    public class SceneSize3D
    {
        public int Item1 { get; set; }
        public int Item2 { get; set; }
        public int Item3 { get; set; }

        public SceneSize3D(int x, int y, int z)
        {
            Item1 = x;
            Item2 = y;
            Item3 = z;
        }

        public SceneSize3D() : this(0, 0, 0)
        {
        }
    }
}
