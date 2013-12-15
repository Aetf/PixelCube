using System.Windows.Media;

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
        /// 是否可见
        /// </summary>
        bool Visible { get; set; }
    }
}
