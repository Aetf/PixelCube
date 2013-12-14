using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PixelCube.Utils
{
    public static class MatixTransform3DExtension
    {
        /// <summary>
        /// 融合另一个Transform3D的变换效果。
        /// </summary>
        /// <param name="obj">本体</param>
        /// <param name="trans">要融合的Transforme3D</param>
        /// <returns>融合之后的本体</returns>
        public static MatrixTransform3D Merge(this MatrixTransform3D obj, Transform3D trans)
        {
            if (trans != null)
            {
                var g = new Transform3DGroup();
                g.Children.Add(obj);
                g.Children.Add(trans);
                obj.Matrix = g.Value;
            }
            return obj;
        }
    }
}
