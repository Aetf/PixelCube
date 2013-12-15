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
        /// 获取于整个世界坐标的变换
        /// </summary>
        MatrixTransform3D WorldTransform { get; }

        /// <summary>
        /// 摄像机的原始坐标
        /// </summary>
        Point3D CameraOrig { get; }

        /// <summary>
        /// 将(i, j, k)的小方块设置为焦点方块。
        /// 非法坐标表示清除焦点。
        /// </summary>
        /// <param name="i">i >= 0 && i < SceneSize.Item1</param>
        /// <param name="j">j >= 0 && j < SceneSize.Item2</param>
        /// <param name="k">k >= 0 && k < SceneSize.Item3</param>
        void SetFocus(int i, int j, int k);

        /// <summary>
        /// 擦除(i, j, k)的小方块.
        /// 非法坐标将抛出ArgumentOutofRangeException
        /// </summary>
        /// <param name="i">i >= 0 && i < SceneSize.Item1</param>
        /// <param name="j">j >= 0 && j < SceneSize.Item2</param>
        /// <param name="k">k >= 0 && k < SceneSize.Item3</param>
        void Erase(int i, int j, int k);

        /// <summary>
        /// 设置(i, j, k)的小方块的颜色。
        /// 非法坐标将抛出ArgumentOutofRangeException
        /// </summary>
        /// <param name="i">i >= 0 && i < SceneSize.Item1</param>
        /// <param name="j">j >= 0 && j < SceneSize.Item2</param>
        /// <param name="k">k >= 0 && k < SceneSize.Item3</param>
        /// <param name="c">要设置的颜色，现在未实现，暂不起作用</param>
        void SetColor(int i, int j, int k, Color c);

        /// <summary>
        /// 控制器的初始化
        /// </summary>
        /// <param name="win">应用主窗口</param>
        void DoInit(MainWindow win);
    }
}
