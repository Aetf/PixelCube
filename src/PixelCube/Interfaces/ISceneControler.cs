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
        /// 摄像机的原始坐标
        /// </summary>
        Point3D CameraOrig { get; }

        /// <summary>
        /// 将(i, j, k)的小方块设置为焦点方块。
        /// 非法坐标表示清除焦点。
        /// </summary>
        /// <param name="i">i >= 0 && i < SceneSize.X</param>
        /// <param name="j">j >= 0 && j < SceneSize.X</param>
        /// <param name="k">k >= 0 && k < SceneSize.X</param>
        void SetFocus(int i, int j, int k);

        /// <summary>
        /// 擦除(i, j, k)的小方块.
        /// 非法坐标将导致异常。
        /// </summary>
        /// <param name="i">i >= 0 && i < SceneSize.X</param>
        /// <param name="j">j >= 0 && j < SceneSize.X</param>
        /// <param name="k">k >= 0 && k < SceneSize.X</param>
        void Erase(int i, int j, int k);

        /// <summary>
        /// 设置(i, j, k)的小方块的颜色。
        /// 非法坐标将抛出ArgumentOutofRangeException
        /// </summary>
        /// <param name="i">i >= 0 && i < SceneSize.X</param>
        /// <param name="j">j >= 0 && j < SceneSize.X</param>
        /// <param name="k">k >= 0 && k < SceneSize.X</param>
        /// <param name="c">为了以后的扩展性，现在传null就好</param>
        void SetColor(int i, int j, int k, Color c);

        /// <summary>
        /// 控制器的初始化
        /// </summary>
        /// <param name="win">应用主窗口</param>
        void DoInit(MainWindow win);
    }
}
