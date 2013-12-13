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

            // Draw a outter frame
            var p000 = new Point3D(0, 0, 0);
            var p001 = new Point3D(0, 0, sceneSize.Z * cubea);
            var p010 = new Point3D(0, sceneSize.Y * cubea, 0);
            var p011 = new Point3D(0, sceneSize.Y * cubea, sceneSize.Z * cubea);
            var p100 = new Point3D(sceneSize.X * cubea, 0, 0);
            var p101 = new Point3D(sceneSize.X * cubea, 0, sceneSize.Z * cubea);
            var p110 = new Point3D(sceneSize.X * cubea, sceneSize.Y * cubea, 0);
            var p111 = new Point3D(sceneSize.X * cubea, sceneSize.Y * cubea, sceneSize.Z * cubea);
            mView.Children.Add(CreateLine(p000, p001));
            mView.Children.Add(CreateLine(p001, p011));
            mView.Children.Add(CreateLine(p011, p111));
            mView.Children.Add(CreateLine(p111, p110));
            mView.Children.Add(CreateLine(p110, p100));
            mView.Children.Add(CreateLine(p100, p101));
            mView.Children.Add(CreateLine(p101, p111));
            mView.Children.Add(CreateLine(p000, p100));
            mView.Children.Add(CreateLine(p000, p010));
            mView.Children.Add(CreateLine(p010, p011));
            mView.Children.Add(CreateLine(p010, p110));
            mView.Children.Add(CreateLine(p001, p101));

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
            var n = new DiffuseMaterial(new SolidColorBrush(Colors.WhiteSmoke));
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

        private LinesVisual3D CreateLine(Point3D from, Point3D to)
        {
            var line = new LinesVisual3D()
            {
                Color = Colors.Gray,
                Points = new Point3D[] { from, to }.ToList()
            };
            return line;
        }
    }
}
