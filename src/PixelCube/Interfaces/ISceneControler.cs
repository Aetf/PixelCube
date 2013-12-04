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
    public interface ISceneControler : IDisposable
    {
        /// <summary>
        /// 获取和设置对于整个世界坐标的变换
        /// </summary>
        Transform3D WorldTransform { get; set; }

        /// <summary>
        /// 将(i, j, k)的小方块设置为焦点方块
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        void SetFocus(int i, int j, int k);

        /// <summary>
        /// 擦除(i, j, k)的小方块
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        void Erase(int i, int j, int k);

        /// <summary>
        /// 控制器的初始化
        /// </summary>
        /// <param name="win">应用主窗口</param>
        void DoInit(MainWindow win);
    }
}
