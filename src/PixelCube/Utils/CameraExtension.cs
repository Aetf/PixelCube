using System.Windows.Media.Media3D;

namespace PixelCube.Utils
{
    public static class CameraExtension
    {
        public static Point3D GetPosition(this Camera camera)
        {
            var porjection = camera as ProjectionCamera;
            var perspective = camera as PerspectiveCamera;
            var orthographic = camera as OrthographicCamera;
            Point3D pos = default(Point3D);
            if (orthographic != null)
            {
                pos = orthographic.Position;
            }
            else if (perspective != null)
            {
                pos = perspective.Position;
            }
            else if (porjection != null)
            {
                pos = porjection.Position;
            }

            return pos;
        }

        public static Vector3D GetUpDirection(this Camera camera)
        {
            var porjection = camera as ProjectionCamera;
            var perspective = camera as PerspectiveCamera;
            var orthographic = camera as OrthographicCamera;
            Vector3D updirection = default(Vector3D);
            if (orthographic != null)
            {
                updirection = orthographic.UpDirection;
            }
            else if (perspective != null)
            {
                updirection = perspective.UpDirection;
            }
            else if (porjection != null)
            {
                updirection = porjection.UpDirection;
            }

            return updirection;
        }

        public static Vector3D GetLookDirection(this Camera camera)
        {
            var porjection = camera as ProjectionCamera;
            var perspective = camera as PerspectiveCamera;
            var orthographic = camera as OrthographicCamera;
            Vector3D lookdirection = default(Vector3D);
            if (orthographic != null)
            {
                lookdirection = orthographic.LookDirection;
            }
            else if (perspective != null)
            {
                lookdirection = perspective.LookDirection;
            }
            else if (porjection != null)
            {
                lookdirection = porjection.LookDirection;
            }

            return lookdirection;
        }
    }
}
