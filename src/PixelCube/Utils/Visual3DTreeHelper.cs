using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace PixelCube.Utils
{
    public static class Visual3DTreeHelper
    {
        /// <summary>
        /// Gets the parent <see cref="Viewport3D"/> from the specified visual.
        /// </summary>
        /// <param name="visual">
        /// The visual.
        /// </param>
        /// <returns>
        /// The Viewport3D
        /// </returns>
        public static Viewport3D GetViewport3D(this Visual3D visual)
        {
            DependencyObject obj = visual;
            while (obj != null)
            {
                var vis = obj as Viewport3DVisual;
                if (vis != null)
                {
                    return VisualTreeHelper.GetParent(obj) as Viewport3D;
                }

                obj = VisualTreeHelper.GetParent(obj);
            }

            return null;
        }
    }
}
