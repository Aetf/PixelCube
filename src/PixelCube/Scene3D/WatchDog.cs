using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using PixelCube.Utils;

namespace PixelCube.Scene3D
{
    public class WatchDog : DependencyObject
    {
        public void DoInit(MainWindow win)
        {
            var mtb = win.infoPanel;

            win.LeapT.TraceEvent += (obj, arg) =>
            {
                Point3D tipPos = arg.TracePosition.ToPoint3D();
                win.Dispatcher.BeginInvoke(new Action(() =>
                {
                    // Tip Position
                    win.pointer.Center = win.SceneControler.WorldTransform.Transform(tipPos);
                    win.pointerxy.Center = new Point3D(win.pointer.Center.X, win.pointer.Center.Y, 0);
                    win.pointeryz.Center = new Point3D(0, win.pointer.Center.Y, win.pointer.Center.Z);
                    win.pointerzx.Center = new Point3D(win.pointer.Center.X, 0, win.pointer.Center.Z);

                    var list = new System.Collections.Generic.List<Point3D>();
                    list.Add(win.pointer.Center); list.Add(win.pointerxy.Center);
                    win.linez.Points = list;
                    list = new System.Collections.Generic.List<Point3D>();
                    list.Add(win.pointer.Center); list.Add(win.pointeryz.Center);
                    win.linex.Points = list;
                    list = new System.Collections.Generic.List<Point3D>();
                    list.Add(win.pointer.Center); list.Add(win.pointerzx.Center);
                    win.liney.Points = list;
                }));
            };

            win.Leap.LeapModeChangeEvent += (obj, arg) =>
            {
                win.Dispatcher.BeginInvoke(new Action(() =>
                {
                    win.modePanel.Text = arg.state.ToString();
                }));
            };

            win.mCamera.Changed += (sender, e) =>
            {
                this.CameraInfo = GetInfo(win.mCamera);
            };
            this.CameraInfo = GetInfo(win.mCamera);
            var binding = new Binding("CameraInfo")
            {
                Source = this
            };
            mtb.SetBinding(TextBlock.TextProperty, binding);
        }

        public static string GetInfo(Camera camera)
        {
            var matrixCamera = camera as MatrixCamera;
            var perspectiveCamera = camera as PerspectiveCamera;
            var projectionCamera = camera as ProjectionCamera;
            var orthographicCamera = camera as OrthographicCamera;
            var sb = new StringBuilder();
            sb.AppendLine(camera.GetType().Name);
            if (projectionCamera != null)
            {
                sb.AppendLine(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "LookDirection:\t{0:0.000},{1:0.000},{2:0.000}",
                        projectionCamera.LookDirection.X,
                        projectionCamera.LookDirection.Y,
                        projectionCamera.LookDirection.Z));
                sb.AppendLine(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "UpDirection:\t{0:0.000},{1:0.000},{2:0.000}",
                        projectionCamera.UpDirection.X,
                        projectionCamera.UpDirection.Y,
                        projectionCamera.UpDirection.Z));
                sb.AppendLine(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Position:\t{0:0.000},{1:0.000},{2:0.000}",
                        projectionCamera.Position.X,
                        projectionCamera.Position.Y,
                        projectionCamera.Position.Z));
                var target = projectionCamera.Position + projectionCamera.LookDirection;
                sb.AppendLine(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Target:\t\t{0:0.000},{1:0.000},{2:0.000}",
                        target.X,
                        target.Y,
                        target.Z));
                sb.AppendLine(
                    string.Format(
                        CultureInfo.InvariantCulture, "NearPlaneDist:\t{0}", projectionCamera.NearPlaneDistance));
                sb.AppendLine(
                    string.Format(CultureInfo.InvariantCulture, "FarPlaneDist:\t{0}", projectionCamera.FarPlaneDistance));
            }

            if (perspectiveCamera != null)
            {
                sb.AppendLine(
                    string.Format(CultureInfo.InvariantCulture, "FieldOfView:\t{0:0.#}°", perspectiveCamera.FieldOfView));
            }

            if (orthographicCamera != null)
            {
                sb.AppendLine(
                    string.Format(CultureInfo.InvariantCulture, "Width:\t{0:0.###}", orthographicCamera.Width));
            }

            if (matrixCamera != null)
            {
                sb.AppendLine("ProjectionMatrix:");
                sb.AppendLine(matrixCamera.ProjectionMatrix.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("ViewMatrix:");
                sb.AppendLine(matrixCamera.ViewMatrix.ToString(CultureInfo.InvariantCulture));
            }

            return sb.ToString().Trim();
        }

        #region public String CameraInfo;
        public static DependencyProperty CameraInfoProperty = DependencyProperty.Register(
            "CameraInfo", typeof(String), typeof(WatchDog), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the camera info.
        /// </summary>
        /// <value>
        /// The camera info.
        /// </value>
        public string CameraInfo
        {
            get { return (string)this.GetValue(CameraInfoProperty); }
            set { this.SetValue(CameraInfoProperty, value); }
        }
        #endregion
    }
}
