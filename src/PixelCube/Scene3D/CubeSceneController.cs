using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

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
            SetFocus(8, 8, 8);
            //SetColor(3, 3, 3, Colors.AntiqueWhite);
        }

        public void SetFocus(int i, int j, int k)
        {
            GradientStopCollection g = new GradientStopCollection();
            Color c1 = new Color();
            c1.ScB = 0;
            c1.ScG = 0;
            c1.ScR = 0;
            c1.ScA = 0;
            GradientStop gs1 = new GradientStop(c1, 0.9);
            //Color c2 = new Color();
            //c2.ScB = 244;
            //c2.ScG = 73;
            //c2.ScR = 23;
            //c2.ScA = 200;
            GradientStop gs2 = new GradientStop(Colors.MediumAquamarine, 1);
            g.Add(gs1);
            g.Add(gs2);
            DiffuseMaterial a = new DiffuseMaterial(new RadialGradientBrush(g));

            GeometryModel3D cube = CubeModelFromIdx(i, j, k);
            cube.Material = a;

            return;
        }

        public void Erase(int i, int j, int k)
        {
            return;
        }
        
        public void SetColor(int i, int j, int k, Color c)
        {
            GeometryModel3D cube = CubeModelFromIdx(i, j, k);

            // FIXME: Only for test.
            //cube.Material = new DiffuseMaterial(Brushes.BlueViolet);
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
            return mWin.FindName(NameForCubeModel(i, j, k)) as GeometryModel3D;
        }

        private String NameForCubeModel(int i, int j, int k)
        {
            return String.Format("CubeModel{0}_{1}_{2}", i, j, k);
        }
    }
}
