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
        private Vector newVec;

        /// <summary>
        /// 获取转换后的坐标
        /// </summary>
        /// <returns>返回转换后得到的世界坐标</returns>
        public Vector getNewVec()
        {
            return newVec;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CoordinatesTrans()
        {
            newVec = new Vector(0,0,0);
            ConfigProvider cp = ConfigProvider.Instance;
            //double cubea = cp.CubeA;

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
                //newVec.x = (vec.x + 300) / () - 100;
                //newVec.y = (vec.y - 30 ) / () - 100;
                //newVec.x = (vec.z + 300) / () - 100;
                return true;
            }
            return false;
        }
    }
}
