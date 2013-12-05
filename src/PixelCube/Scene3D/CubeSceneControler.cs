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
                mView.Dispatcher.BeginInvoke(new Action(
                    () => 
                    {
                        mView.Camera.Transform = value;
                    })
                    , null);
            }
        }

        public void DoInit(MainWindow win)
        {
            mView = win.getViewport();
            Model3DGroup group = win.getCubeGroup();
            
            var cubeseed = (GeometryModel3D)win.FindResource("cubeSeed");
            var cubea = (double)win.FindResource("cubeA");
            //var sceneSize = win.CurrentArt.SceneSize;
            Vector3D sceneSize = new Vector3D(4, 4, 4);

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
            return;
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
