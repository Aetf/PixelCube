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


namespace PixelCube.OpCore
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

        #endregion

        private IArtwork martwork;
        private int mcubea;//小方块的边长
        private ISceneControler msceneController;

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
            martwork = win.CurrentArt;
            msceneController = win.SceneControler;
            //获取小方块的边长
            mcubea = (int)win.FindResource("cubeA");
        }

        #region 事件响应函数
        /// <summary>
        /// 焦点变化响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnPreFocusOperation(object sender, PreFocusOperationEventArgs e)
        {
            Vector curPosition = e.FocusPosition;
            //x,y,z为小方块的绝对三维坐标
            int x = (int)curPosition.x;
            int y = (int)curPosition.y;
            int z = (int)curPosition.z;

            //i,j,k为小方块的三维位置索引
            int i = x / mcubea;
            int j = y / mcubea;
            int k = z / mcubea;
            //计算出小方块在链表中的索引
            int index = i + (int)martwork.SceneSize.X * j + (int)martwork.SceneSize.Y * k;
            
            //获取指定小方块
            ICube cube = martwork.Cubes[index];

            //修改其为高亮颜色
            //cube.CubeColor = Colors.Blue;

            //发出事件
            if (PostFocusOperationEvent != null)
            {
                PostFocusOperationEvent(this, new PostFocusOperationEventArgs(index));
            }
        }

        /// <summary>
        /// 着色响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnPreDrawOperation(object sender, PreDrawOperationEventArgs e)
        {
            Vector drawPosition = e.DrawPosition;
            //i,j,k为小方块的三维位置索引
            int i = (int)drawPosition.x / mcubea;
            int j = (int)drawPosition.y / mcubea;
            int k = (int)drawPosition.z / mcubea;

            //计算小方块在链表中的索引
            int index = i + (int)martwork.SceneSize.X * j + (int)martwork.SceneSize.Y * k;

            //获取要图画的小方块
            ICube cube = martwork.Cubes[index];

            //修改小方块上色的颜色
            //...

            //发出要图画事件
            if (PostDrawOperationEvent != null) {
                PostDrawOperationEvent(this, new PostDrawOperationEventArgs(index));
            }
        }

        /// <summary>
        /// 旋转变化响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnPreRotateOperation(object sender, PreRotateOperationEventArgs e)
        {
            //从事件参数中获取旋转轴向量
            float[] vector = e.RotationAxis.ToFloatArray();
            //转化为用C#提供的向量类型表示
            Vector3D rotateAxis = new Vector3D(vector[0], vector[1], vector[2]);
            //从事件参数中获取旋转角度
            float rotateAngel = e.RotationAngle;
            //定义绕轴旋转变换
            AxisAngleRotation3D axisAngelRotation = new AxisAngleRotation3D(rotateAxis, rotateAngel);
            //根据变换定义变换矩阵
            RotateTransform3D rotateTransform = new RotateTransform3D(axisAngelRotation);
            //传递给视图控制类
            msceneController.WorldTransform = rotateTransform;
            //发出事件
            if (PostRotateOperationEvent != null)
            {
                PostRotateOperationEvent(this, new PostRotateOperationEventArgs());
            }
        }

        /// <summary>
        /// 缩放响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnPreScaleOperation(object sender, PreScaleOperationEventArgs e)
        {
        }

        /// <summary>
        /// 颜色变换响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnChangeColorOperation(object sender, PreChangeColorOperationEventArgs e)
        {
        }

        /// <summary>
        /// 平移响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnDragOperation(object sender, PreDragOperationEventArgs e)
        {
        }

        /// <summary>
        /// 擦出响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnEraseOperation(object sender, PreEraseOperationEventArgs e)
        {

        }
        #endregion
        #endregion
    }
}
