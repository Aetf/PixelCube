using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using PixelCube.Scene3D;
namespace PixelCube.LeapMotion
{
    class CoordinatesTrans
    {
        /// <summary>
        /// 转换后的新坐标
        /// </summary>
        private Vector newVec;

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
            newVec = new Vector(0, 0, 0);
            ConfigProvider cp = ConfigProvider.Instance;
            cubea = cp.CubeA;       //获取小方块边长

            cuben = art.SceneSize.Item1;    //获取每行小方块数目
            maxCoord =(float) cubea * cuben;
            maxCoord += 2 * maxCoord;

        }

        /// <summary>
        /// 获取转换后的坐标
        /// </summary>
        /// <returns>返回转换后得到的世界坐标</returns>
        public Vector getNewVec()
        {
            return newVec;
        }

        /// <summary>
        /// 将LeapMotion坐标点转换为世界坐标点
        /// </summary>
        /// <param name="vec">LeapMotion坐标点</param>
        /// <param name="maxX">世界坐标X轴最大值</param>
        /// <param name="maxY">世界坐标Y轴最大值</param>
        /// <param name="maxZ">世界坐标Z轴最大值</param>
        /// <returns>变换是否成功</returns>
        public bool TransPoint(Vector vec)
        {
            if (vec.x >= -300 && vec.x <= 300 && vec.y >= 30 && vec.y <= 630 && vec.z >= -300 && vec.z <= 300)
            {
                //operations:
                newVec.x = (vec.x + 300) / (600 / maxCoord) - maxCoord/3 ;
                newVec.y = (vec.y - 30) / (600 / maxCoord) - maxCoord/3 ;
                newVec.z = (vec.z + 300) / (600 / maxCoord) - maxCoord/3 ;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 将LeapMotion坐标偏移量转换为世界坐标偏移量
        /// </summary>
        /// <param name="vec"></param>
        /// <returns>true</returns>
        public bool TransVector(Vector vec)
        {
            vec.x = vec.x / 200 * maxCoord;
            vec.y = vec.y / 200 * maxCoord;
            vec.z = vec.z / 200 * maxCoord;
            return true;
        }
    }
}
