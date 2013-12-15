using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PixelCube.Utils
{
    public static class LeapVectorExtionsion
    {
        public static Point3D ToPoint3D(this Leap.Vector vec)
        {
            return new Point3D(vec.x, vec.y, vec.z);
        }

        public static Vector3D ToVector3D(this Leap.Vector vec)
        {
            return new Vector3D(vec.x, vec.y, vec.z);
        }
    }
}
