using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Diagnostics;
using System.Linq;
using PixelCube.Utils;

namespace PixelCube.Scene3D
{
    class CubeSceneController : ISceneControler
    {
        public void TranslateCamera(Vector3D offset)
        {
            mView.Camera.Position = Point3D.Add(mView.Camera.Position, offset);
        }

        public void RotateCamera(RotateTransform3D rotation)
        {
            mView.Camera.Position = rotation.Transform(mView.Camera.Position);
            mView.Camera.UpDirection = rotation.Transform(mView.Camera.UpDirection);
            mView.Camera.LookDirection = rotation.Transform(mView.Camera.LookDirection);
        }

        #region ISceneControler 成员

        private Transform3D worldTransform = Transform3D.Identity;
        public Transform3D WorldTransform
        {
            get
            {
                return worldTransform;
            }
            set
            {
                worldTransform = value;
                //mView.Camera.Transform = worldTransform;
                //mView.Camera.LookDirection = value.Transform(mView.Camera.LookDirection);
                mView.Camera.Position = value.Transform(mView.Camera.Position);
                //mView.Camera.UpDirection = value.Transform(mView.Camera.UpDirection);
            }
        }

        public Point3D CameraOrig
        {
            get
            {
                return (mView.Camera as PerspectiveCamera).Position;
            }
        }

        public void DoInit(MainWindow win)
        {
            mWin = win;
            mView = win.getViewport();
            Model3DGroup group = win.getCubeGroup();
            
            var cubeseed = (GeometryModel3D)win.FindResource("cubeSeed");
            var cubea = (double)win.FindResource("cubeA");
            var sceneSize = win.CurrentArt.SceneSize;
            //Vector3D sceneSize = new Vector3D(4, 4, 4);

            NameScope.SetNameScope(win, new NameScope());
            for (int i = 0; i != sceneSize.X; i++)
            {
                for (int j = 0; j != sceneSize.Y; j++)
                {
                    for (int k = 0; k != sceneSize.Z; k++)
                    {
                        GeometryModel3D c = cubeseed.Clone();
                        mWin.RegisterName(NameForCubeModel(i, j, k), c);
                        c.Transform = new TranslateTransform3D(cubea*i, cubea*j, cubea*k);
                        group.Children.Add(c);
                    }
                }
            }
            //SetColor(2, 2, 2, default(Color));
            //SetColor(3, 3, 3, default(Color));
            //SetFocus(2, 2, 2);
            //SetFocus(3, 3, 3);
            //SetFocus(-1, -1, -1);
            //SetColor(3, 3, 3, Colors.AntiqueWhite);
        }

        Tuple<int, int, int> preFocus = null;
        public void SetFocus(int i, int j, int k)
        {
            // Clear previous focus
            if (preFocus != null)
            {
                GeometryModel3D preCube = CubeModelFromIdx(preFocus.Item1, preFocus.Item2, preFocus.Item3);
                var g = preCube.Material as MaterialGroup;
                var focusmaterial = g.Children.OfType<EmissiveMaterial>().LastOrDefault();
                g.Children.Remove(focusmaterial);
                preFocus = null;
            }
            // Set focus
            if (!(i < 0 || j < 0 || k < 0))
            {
                Debug.WriteLine(String.Format("Focus: {0}, {1}, {2}", i, j, k));

                GeometryModel3D cube = CubeModelFromIdx(i, j, k);
                preFocus = Tuple.Create(i, j, k);
                var g = cube.Material as MaterialGroup;
                g.Children.Add(mWin.FindResource("focusMaterial") as Material);
            }
        }

        public void Erase(int i, int j, int k)
        {
            var cube = CubeModelFromIdx(i, j, k);

            var g = cube.Material as MaterialGroup;
            var old = g.Children.OfType<DiffuseMaterial>().First();
            g.Children.Remove(old);

            var n = mWin.FindResource("whiteSmokeMaterial") as Material;
            g.Children.Insert(0, n);
        }
        
        public void SetColor(int i, int j, int k, Color c)
        {
            var cube = CubeModelFromIdx(i, j, k);

            var g = cube.Material as MaterialGroup;
            var old = g.Children.OfType<DiffuseMaterial>().First();
            g.Children.Remove(old);

            // FIXME: only debug propose here!! Should use c in release.
            //var n = new DiffuseMaterial(new SolidColorBrush(c));
            var n = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));
            g.Children.Insert(0, n);
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            return;
        }

        #endregion

        private HelixViewport3D mView;
        private MainWindow mWin;

        public CubeSceneController()
        {
            
        }

        private GeometryModel3D CubeModelFromIdx(int i, int j, int k)
        {
            var c = mWin.FindName(NameForCubeModel(i, j, k)) as GeometryModel3D;
            if (c == null)
                throw new ArgumentOutOfRangeException("(i, j, k)", new Vector3D(i, j, k), "Shoule be >=0 && < SceneSize.X/Y/Z");
            return c;
        }

        private String NameForCubeModel(int i, int j, int k)
        {
            return String.Format("CubeModel{0}_{1}_{2}", i, j, k);
        }
    }
}
