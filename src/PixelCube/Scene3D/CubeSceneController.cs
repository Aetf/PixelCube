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
        

        #region ISceneControler 成员
        public void TranslateCamera(Vector3D offset)
        {
            mView.Camera.Position = Point3D.Add(mView.Camera.Position, offset);
            WorldTransform.Merge(new TranslateTransform3D(offset));
        }

        public void RotateCamera(RotateTransform3D rotation)
        {
            mView.Camera.Position = rotation.Transform(mView.Camera.Position);
            mView.Camera.UpDirection = rotation.Transform(mView.Camera.UpDirection);
            mView.Camera.LookDirection = rotation.Transform(mView.Camera.LookDirection);

            WorldTransform.Merge(rotation);
        }

        public MatrixTransform3D WorldTransform { get; set; }

        public Point3D CameraOrig
        {
            get
            {
                return (mView.Camera as PerspectiveCamera).Position;
            }
        }

        public void DoInit(MainWindow win)
        {
            WorldTransform = new MatrixTransform3D(Matrix3D.Identity);

            mWin = win;
            mView = win.getViewport();
            Model3DGroup group = win.getCubeGroup();
            
            var cubeseed = (GeometryModel3D)win.FindResource("cubeSeed");
            var cubea = (double)win.FindResource("cubeA");
            var sceneSize = win.CurrentArt.SceneSize;

            // Read cube list from artwork
            cubeModels = SerializeHepler.cubeInput(cubeseed, win.CurrentArt).ToArray();

            NameScope.SetNameScope(win, new NameScope());
            for (int i = 0; i != sceneSize.X; i++)
            {
                for (int j = 0; j != sceneSize.Y; j++)
                {
                    for (int k = 0; k != sceneSize.Z; k++)
                    {
                        GeometryModel3D c = cubeModels[(int)(i + sceneSize.X * j + sceneSize.X * sceneSize.Y * k)];
                        mWin.RegisterName(NameForCubeModel(i, j, k), c);
                        c.Transform = new TranslateTransform3D(cubea*i, cubea*j, cubea*k);
                        group.Children.Add(c);
                    }
                }
            }
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

        public void Flush()
        {
            mWin.CurrentArt.Cubes = SerializeHepler.cubeOutput(cubeModels);
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
        private GeometryModel3D[] cubeModels;

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
