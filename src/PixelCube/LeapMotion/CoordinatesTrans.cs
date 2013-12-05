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
        /// 构造函数
        /// </summary>
        public CoordinatesTrans()
        {
        }
        /// <summary>
        /// 将LeapMotion坐标转换为世界坐标
        /// </summary>
        /// <param name="vec">LeapMotion坐标</param>
        /// <param name="maxX">世界坐标X轴最大值</param>
        /// <param name="maxY">世界坐标Y轴最大值</param>
        /// <param name="maxZ">世界坐标Z轴最大值</param>
        /// <returns>变换后的坐标</returns>
        public void Trans(Vector vec, int maxX, int maxY, int maxZ)
        {

        }

    }
}
