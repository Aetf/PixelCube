using System.Windows.Media.Media3D;

namespace PixelCube.Utils
{
    static public class GeometryExtension
    {
        /// <summary>
        /// 获取一个Rect3D的几何中心
        /// </summary>
        /// <param name="rect3d"></param>
        /// <returns>几何中心</returns>
        public static Point3D GetCenter(this Rect3D rect3d)
        {
            var tocenter = new Vector3D(rect3d.Size.X, rect3d.Size.Y, rect3d.Size.Z);
            tocenter = Vector3D.Divide(tocenter, 2);
            return Point3D.Add(rect3d.Location, tocenter);
        }

        /// <summary>
        /// 添加一个新的变换到Model3D上，并保留原有Transform3D的变换效果。
        /// </summary>
        /// <param name="geo">操作的Model3D</param>
        /// <param name="trans">添加的Transform3D</param>
        public static void AddTransform(this Model3D geo, Transform3D trans)
        {
            if(geo.Transform != null)
            {
                MatrixTransform3D mt = new MatrixTransform3D(geo.Transform.Value);
                geo.Transform = mt.Merge(trans);
            }
            else
            {
                geo.Transform = trans;
            }
        }

        /// <summary>
        /// 添加一个新的变换到Visual3D上，并保留原有Transform3D的变换效果。
        /// </summary>
        /// <param name="visual">操作的Visual3D</param>
        /// <param name="trans">添加的Transform3D</param>
        public static void AddTransform(this Visual3D visual, Transform3D trans)
        {
            if (visual.Transform != null)
            {
                MatrixTransform3D mt = new MatrixTransform3D(visual.Transform.Value);
                visual.Transform = mt.Merge(trans);
            }
            else
            {
                visual.Transform = trans;
            }
        }

        /// <summary>
        /// 将一个GeometryModel3D形状中心移动至坐标原点
        /// </summary>
        /// <param name="geoM">需要移动的GeometryModel3D</param>
        /// <returns>此次移动施加的变换</returns>
        public static Transform3D MakeSureCenterZero(this GeometryModel3D geoM)
        {
            var geo = geoM.Geometry;
            if(geo != null)
            {
	            var offset = Point3D.Subtract(new Point3D(0, 0, 0),
                                               geo.Bounds.GetCenter());
                var trans = new TranslateTransform3D(offset);
	            geoM.AddTransform(trans);
                return trans;
            }
            return Transform3D.Identity;
        }
    }
}
