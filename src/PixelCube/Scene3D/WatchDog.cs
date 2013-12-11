using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using PixelCube.LeapMotion;

namespace PixelCube.Scene3D
{
    public class WatchDog
    {
        TextBlock mtb;

        public void DoInit(MainWindow win)
        {
            mtb = win.infoPanel;

            win.Leap.PreFocusOperationEvent += new System.EventHandler<PreFocusOperationEventArgs>((obj, arg) =>
            {
                win.Dispatcher.BeginInvoke(new Action(() =>
                {
                    // Tip Position
                    win.pointer.Center = win.SceneControler.WorldTransform.Transform(
                        new Point3D(arg.FocusPosition.x, arg.FocusPosition.y, arg.FocusPosition.z));
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
            });
        }
    }
}
