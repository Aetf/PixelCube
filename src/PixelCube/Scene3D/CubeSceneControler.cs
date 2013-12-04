using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

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
                mView.Camera.Transform = value;
            }
        }

        public void DoInit(MainWindow win)
        {
            mView = win.getViewport();
            Model3DGroup group = win.getCubeGroup();
            
            var cubeseed = (GeometryModel3D)win.FindResource("cubeSeed");
            var cubea = (double)win.FindResource("cubeA");
            var sceneSize = win.CurrentArt.SceneSize;

            for(int i = 0; i!= sceneSize.X; i++)
            {
                for(int j = 0; j!= sceneSize.Y; j++)
                {
                    for(int k = 0; k!= sceneSize.Z; k++)
                    {
                        GeometryModel3D c = cubeseed.Clone();
                        c.Transform = new TranslateTransform3D(cubea*i, cubea*j, cubea*k);
                        group.Children.Add(c);
                    }
                }
            }
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            return;
        }

        #endregion

        private Viewport3D mView;

        public CubeSceneControler()
        {
        }
    }
}
