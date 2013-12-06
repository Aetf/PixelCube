using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
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
        /// 构造函数
        /// </summary>
        public CoordinatesTrans()
        {
            newVec = new Vector(0, 0, 0);
            ConfigProvider cp = ConfigProvider.Instance;
            cubea = cp.CubeA;       //获取小方块边长

            //在此处获取当前每行小方块数目

            maxCoord =(float) cubea * cuben + 40;
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
        /// 将LeapMotion坐标转换为世界坐标
        /// </summary>
        /// <param name="vec">LeapMotion坐标</param>
        /// <param name="maxX">世界坐标X轴最大值</param>
        /// <param name="maxY">世界坐标Y轴最大值</param>
        /// <param name="maxZ">世界坐标Z轴最大值</param>
        /// <returns>变换后的坐标</returns>
        public bool Trans(Vector vec)
        {
            if (vec.x >= -300 && vec.x <= 300 && vec.y >= 30 && vec.y <= 630 && vec.z >= -300 && vec.z <= 300)
            {
                //operations:
                newVec.x = (vec.x + 300) / (600 / maxCoord) - maxCoord/2 - 20;
                newVec.y = (vec.y - 30) / (600 / maxCoord) - 20;
                newVec.x = (vec.z + 300) / (600 / maxCoord) - maxCoord/2 - 20;
                return true;
            }
            return false;
        }
    }
}
