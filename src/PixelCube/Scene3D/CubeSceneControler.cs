using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace PixelCube.Scene3D
{
    class CubeSceneControler : ISceneControler
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
                mView.Dispatcher.BeginInvoke(new Action(
                    () => 
                    {
                        mView.Camera.Transform = value;
                    })
                    , null);
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
            //var sceneSize = win.CurrentArt.SceneSize;
            Vector3D sceneSize = new Vector3D(4, 4, 4);

            NameScope.SetNameScope(win, new NameScope());
            for(int i = 0; i!= sceneSize.X; i++)
            {
                for(int j = 0; j!= sceneSize.Y; j++)
                {
                    for(int k = 0; k!= sceneSize.Z; k++)
                    {
                        GeometryModel3D c = cubeseed.Clone();
                        mWin.RegisterName(NameForCubeModel(i, j, k), c);
                        c.Transform = new TranslateTransform3D(cubea*i, cubea*j, cubea*k);
                        group.Children.Add(c);
                    }
                }
            }

            SetColor(3, 3, 3, Colors.AntiqueWhite);
        }

        public void SetFocus(int i, int j, int k)
        {
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
            //cube.Material = new DiffuseMaterial(Brushes.Red);
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

        public CubeSceneControler()
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
