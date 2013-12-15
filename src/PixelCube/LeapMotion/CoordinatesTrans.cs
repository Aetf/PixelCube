using Leap;
using PixelCube.Scene3D;
using System.Windows.Media.Media3D;
namespace PixelCube.LeapMotion
{
    class CoordinatesTrans
    {
        /// <summary>
        /// 小方块边长
        /// </summary>
        private double cubea;

        /// <summary>
        /// 每行小方块数目
        /// </summary>
        private int cuben;

        /// <summary>
        /// 世界坐标的最大范围
        /// </summary>
        private float maxCoord;

        /// <summary>
        /// 带参构造
        /// </summary>
        /// <param name="art">Artwork 的对象</param>
        public CoordinatesTrans(IArtwork art)
        {
            ConfigProvider cp = ConfigProvider.Instance;
            cubea = cp.CubeA;       //获取小方块边长

            cuben = art.SceneSize.Item1;    //获取每行小方块数目
            maxCoord =(float) cubea * cuben;
            maxCoord += 2 * maxCoord;

        }

        /// <summary>
        /// 将LeapMotion坐标点转换为世界坐标点
        /// </summary>
        /// <param name="vec">LeapMotion坐标点</param>
        /// <param name="maxX">世界坐标X轴最大值</param>
        /// <param name="maxY">世界坐标Y轴最大值</param>
        /// <param name="maxZ">世界坐标Z轴最大值</param>
        /// <returns>变换是否成功</returns>
        public Point3D TransPoint(Vector vec)
        {
            var pos = new Point3D();
            //operations:
            pos.X = (vec.x + 300) / (600 / maxCoord) - maxCoord / 3;
            pos.Y = (vec.y - 30) / (600 / maxCoord) - maxCoord / 3;
            pos.Z = (vec.z + 300) / (600 / maxCoord) - maxCoord / 3;
            return pos;
        }

        public bool CheckBound(Vector vec)
        {
            return vec.x >= -300 && vec.x <= 300
                && vec.y >= 30 && vec.y <= 630
                && vec.z >= -300 && vec.z <= 300;
        }

        /// <summary>
        /// 将LeapMotion坐标偏移量转换为世界坐标偏移量
        /// </summary>
        /// <param name="vec"></param>
        /// <returns>true</returns>
        public Vector3D TransVector(Vector vec)
        {
            Vector3D v = new Vector3D();
            v.X = vec.x / 200 * maxCoord;
            v.Y = vec.y / 200 * maxCoord;
            v.Z = vec.z / 200 * maxCoord;
            return v;
        }
    }
}
