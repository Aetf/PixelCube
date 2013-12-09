using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Leap;
using PixelCube.LeapMotion;
using PixelCube.Scene3D;
using PixelCube.Utils;
using System.Diagnostics;


namespace PixelCube.Operations
{

    /// <summary>
    /// 操作核心实现类
    /// </summary>
    public class OpCore
    {
        #region 成员变量
        #region 核心操作类的事件
        /// <summary>
        /// 焦点变化事件
        /// </summary>
        public event EventHandler<PostFocusOperationEventArgs> PostFocusOperationEvent;

        /// <summary>
        /// 小方块着色事件
        /// </summary>
        public event EventHandler<PostDrawOperationEventArgs> PostDrawOperationEvent;

        /// <summary>
        /// 世界角度变换事件
        /// </summary>
        public event EventHandler<PostRotateOperationEventArgs> PostRotateOperationEvent;

        /// <summary>
        /// 世界缩放事件
        /// </summary>
        public event EventHandler<PostScaleOperationEventArgs> PostScaleOperationEvent;

        /// <summary>
        /// 世界平移事件
        /// </summary>
        public event EventHandler<PostDragOperationEventArgs> PostDragOperationEvent;

        #endregion

        private double mcubea;//小方块的边长
        private ISceneControler msceneController;
        private MainWindow mwin;
        private IArtwork martwork;
        private int[] dragFactor = { 0, 0, 0 };//记录累计平移的向量，判断用户是否平移太远

        #endregion

        public OpCore()
        {
        }
        #region 成员方法

        /// <summary>
        /// 初始化函数
        /// </summary>
        /// <param name="win">框架实例</param>
        public void DoInit(MainWindow win)
        {
            mwin = win;
            martwork = win.CurrentArt;
            msceneController = win.SceneControler;
            //获取小方块的边长
            mcubea = (double)win.FindResource("cubeA");
        }

        #region 事件响应函数
        /// <summary>
        /// 焦点变化响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnPreFocusOperation(object sender, PreFocusOperationEventArgs e)
        {
            mwin.Dispatcher.BeginInvoke(new Action(() =>
            {
                Debug.WriteLine("Focus handle");
                Vector curLPPosition = e.FocusPosition;
                Point3D curPosition = new Point3D(curLPPosition.x, curLPPosition.y, curLPPosition.z);
                //获取当前累计变换矩阵的逆矩阵
                GeneralTransform3D transform = msceneController.WorldTransform.Inverse;
                //对当前坐标进行逆变换
                transform.Transform(curPosition);
                //x,y,z为小方块的绝对三维坐标
                //int x = (int);
                //int y = (int);
                //int z = (int);

                //i,j,k为小方块的三维位置索引
                int i = (int)(curPosition.X / mcubea);
                int j = (int)(curPosition.Y / mcubea);
                int k = (int)(curPosition.Z / mcubea);

                //判断当前坐标是否越界
                if (i < martwork.SceneSize.X
                    && j < martwork.SceneSize.Y
                    && k < martwork.SceneSize.Z)
                {
                    //通知视图控制类
                    msceneController.SetFocus(i, j, k);
                }
                else
                {
                    //去除焦点
                    msceneController.SetFocus(-1, -1, -1);
                }
            }), null);
        }

        /// <summary>
        /// 着色响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnPreDrawOperation(object sender, PreDrawOperationEventArgs e)
        {
            mwin.Dispatcher.BeginInvoke(new Action(() =>
            {
                //通过事件参数获取上色小方块坐标
                Vector drawPosition = e.DrawPosition;

                //将leapmotion捕捉到的小方块坐标封装
                Point3D inCameraPosition = new Point3D(drawPosition.x, drawPosition.y, drawPosition.z);

                //将摄像机看到的坐标转换为小方块的绝对三维坐标
                GeneralTransform3D transform = msceneController.WorldTransform.Inverse;
                //进行转换
                transform.Transform(inCameraPosition);

                //i,j,k为小方块的三维位置索引
                int i = (int)(inCameraPosition.X / mcubea);
                int j = (int)(inCameraPosition.Y / mcubea);
                int k = (int)(inCameraPosition.Z / mcubea);

                //设置小方块上色的颜色，目前为默认值
                Color c = new Color();

                //判断小方块绝对三维坐标是否离开画布
                if (i < martwork.SceneSize.X && j <= martwork.SceneSize.Y && k < martwork.SceneSize.Z)
                {
                    //修改小方块上色的颜色
                    msceneController.SetColor(i, j, k, c);

                    //发出上色效果音触发事件
                    if (PostDrawOperationEvent != null)
                    {
                        PostDrawOperationEvent(this, new PostDrawOperationEventArgs(0));
                    }
                }
                else
                {
                    //不上色
                    msceneController.SetColor(-1, -1, -1, c);
                }
            }), null);
        }

        /// <summary>
        /// 旋转变化响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnPreRotateOperation(object sender, PreRotateOperationEventArgs e)
        {
            mwin.Dispatcher.BeginInvoke(new Action(() =>
            {
                Debug.WriteLine("Rotate Handler");
                //从事件参数中获取旋转轴向量
                float[] vector = e.RotationAxis.ToFloatArray();
                //转化为用C#提供的向量类型表示
                Vector3D rotateAxis = new Vector3D(vector[0], vector[1], vector[2]);
                //从事件参数中获取旋转角度
                double rotateAngel = e.RotationAngle;
                rotateAngel *= 180 / Math.PI; // from rad to deg
                //定义绕轴旋转变换
                AxisAngleRotation3D axisAngelRotation = new AxisAngleRotation3D(rotateAxis, rotateAngel);
                //根据变换定义变换矩阵
                RotateTransform3D rotateTransform = new RotateTransform3D(axisAngelRotation);
                //从试图控制类中获取当前累计变换矩阵
                MatrixTransform3D mt = new MatrixTransform3D(msceneController.WorldTransform.Value);
                //把本次旋转的矩阵加入到累计变换矩阵中
                mt.Merge(rotateTransform);
                //传递给视图控制类
                msceneController.WorldTransform = mt;

                //发出事件
                if (PostRotateOperationEvent != null)
                {
                    PostRotateOperationEvent(this, new PostRotateOperationEventArgs());
                }
            }), null);
        }

        /// <summary>
        /// 缩放响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnPreScaleOperation(object sender, PreScaleOperationEventArgs e)
        {
            mwin.Dispatcher.BeginInvoke(new Action(() =>
            {
                //从事件参数中获取缩放程度参数
                float scaleFactor = e.ScaleFactor;

                //计算出摄像机变换矩阵缩放的参数，即取需要缩放大小的倒数
                float inCameraScaleFactor = 1 / scaleFactor;

                //完成世界矩阵的缩放,改变摄像机的变换矩阵实现缩放，并且以当前显示场景的坐标原点为中心
                //建立新的临时累计矩阵
                MatrixTransform3D worldTransform = new MatrixTransform3D(msceneController.WorldTransform.Value);

                //将缩放转换矩阵融入累计变换矩阵
                worldTransform.Merge(new ScaleTransform3D(new Vector3D(inCameraScaleFactor, inCameraScaleFactor, inCameraScaleFactor), msceneController.WorldTransform.Transform(new Point3D(0, 0, 0))));

                //更新累计变换矩阵
                msceneController.WorldTransform = worldTransform;

                //触发缩放完成事件，播放效果音
                if (PostScaleOperationEvent != null)
                {
                    PostScaleOperationEvent(this, new PostScaleOperationEventArgs());
                }
            }), null);
        }

        /// <summary>
        /// 颜色变换响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnChangeColorOperation(object sender, PreChangeColorOperationEventArgs e)
        {
            mwin.Dispatcher.BeginInvoke(new Action(() =>
            {
            }), null);
        }

        /// <summary>
        /// 平移响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnDragOperation(object sender, PreDragOperationEventArgs e)
        {
            mwin.Dispatcher.BeginInvoke(new Action(() =>
            {
                //从事件参数中获取平移向量参数
                Vector transVector = e.TransVector;

                //i,j,k为小方块的三维位置索引,用来与画布中小方块个数比较，判断是否平移太远，若是，则取消此次平移
                int i = (int)(transVector.x / mcubea);
                int j = (int)(transVector.y / mcubea);
                int k = (int)(transVector.z / mcubea);

                //假设能够平移，更新累计向量
                dragFactor[0] += i;
                dragFactor[1] += j;
                dragFactor[2] += k;

                //比较累计平移向量与画布大小，判断平移是否离开画布区域，若否则进行平移，若是则取消平移，取消累计平移向量的更新
                if (dragFactor[0] < martwork.SceneSize.X && dragFactor[1] < martwork.SceneSize.Y && dragFactor[2] < martwork.SceneSize.Z && dragFactor[0] > -martwork.SceneSize.X && dragFactor[1] > -martwork.SceneSize.Y && dragFactor[2] > -martwork.SceneSize.Z)
                {

                    //封装为三维向量,并转换为对摄像机的转换矩阵的平移向量参数，即对原平移向量坐标取反
                    Vector3D inCameraTransVector = new Vector3D(-transVector.x, -transVector.y, -transVector.z);

                    //完成摄像机的转换矩阵的转换，实现场景的平移
                    //建立新的临时累计矩阵
                    MatrixTransform3D worldTransform = new MatrixTransform3D(msceneController.WorldTransform.Value);

                    //将缩放转换矩阵融入累计变换矩阵
                    worldTransform.Merge(new TranslateTransform3D(inCameraTransVector));

                    //更新累计变换矩阵
                    msceneController.WorldTransform = worldTransform;

                    //触发完成平移事件，发出效果音
                    if (PostDragOperationEvent != null)
                    {
                        PostDragOperationEvent(this, new PostDragOperationEventArgs());
                    }
                }
                else {
                    //取消累计平移向量的更新
                    dragFactor[0] -= i;
                    dragFactor[1] -= j;
                    dragFactor[2] -= k;
                }
            }), null);
        }

        /// <summary>
        /// 擦出响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnEraseOperation(object sender, PreEraseOperationEventArgs e)
        {
            mwin.Dispatcher.BeginInvoke(new Action(() =>
            {
                Vector curLPPosition = e.Position;
                Point3D curPosition = new Point3D(curLPPosition.x, curLPPosition.y, curLPPosition.z);
                //获取当前累计变换矩阵的逆矩阵
                GeneralTransform3D transform = msceneController.WorldTransform.Inverse;
                //对当前坐标进行逆变换
                transform.Transform(curPosition);
                //x,y,z为小方块的绝对三维坐标
                //int x = (int);
                //int y = (int);
                //int z = (int);

                //i,j,k为小方块的三维位置索引
                int i = (int)(curPosition.X / mcubea);
                int j = (int)(curPosition.Y / mcubea);
                int k = (int)(curPosition.Z / mcubea);

                //判断当前坐标是否越界
                if (i < martwork.SceneSize.X
                    && j < martwork.SceneSize.Y
                    && k < martwork.SceneSize.Z)
                {
                    //通知试图控制类
                    msceneController.Erase(i, j, k);
                }
            }), null);
        }
        #endregion
        #endregion
    }
}
