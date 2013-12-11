using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Diagnostics;

namespace PixelCube.Scene3D
{
    class CubeSceneController : ISceneControler
    {
        #region ISceneControler 成员

        public Transform3D WorldTransform
        {
            get
            {
                return mView.Camera.Transform;
            }
            set
            {
                mView.Camera.Transform = value;
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
            //SetFocus(2, 2, 2);
            //SetFocus(3, 3, 3);
            //SetFocus(-1, -1, -1);
            //SetColor(3, 3, 3, Colors.AntiqueWhite);
        }

        static const Vector3D InvalidDefault = new Vector3D(-1, -1, -1);
        Vector3D preFocus = InvalidDefault;
        Material preMaterial = default(Material);
        public void SetFocus(int i, int j, int k)
        {
            // Clear previous focus
            if (!preFocus.Equals(new Vector3D(-1, -1, -1)))
            {
                int x = (int)preFocus.X;
                int y = (int)preFocus.Y;
                int z = (int)preFocus.Z;
                GeometryModel3D preCube = CubeModelFromIdx(x, y, z);
                preCube.Material = preMaterial;
                preFocus = InvalidDefault;
            }
            // Set focus
            if (!(i < 0 || j < 0 || k < 0))
            {
                GeometryModel3D cube = CubeModelFromIdx(i, j, k);
                preFocus = new Vector3D(i, j, k);
                preMaterial = cube.Material;
                cube.Material = mWin.FindResource("focusMaterial") as Material;
            }
        }

        public void Erase(int i, int j, int k)
        {
            var cube = CubeModelFromIdx(i, j, k);
            return;
        }
        
        public void SetColor(int i, int j, int k, Color c)
        {
            var cube = CubeModelFromIdx(i, j, k);

            // FIXME: only debug propose here!! Should use c in release.
            //cube.Material = new DiffuseMaterial(new SolidColorBrush(c));
            cube.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));
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
