using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
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

        public MatrixTransform3D WorldTransform { get; private set; }

        public Point3D CameraOrig
        {
            get { return (mView.Camera as PerspectiveCamera).Position; }
        }

        public void DoInit(MainWindow win)
        {
            WorldTransform = new MatrixTransform3D(Matrix3D.Identity);

            mWin = win;
            mView = win.sceneViewport;
            mCubeGroup = win.cubeGroup;
            
            var cubeseed = (GeometryModel3D)win.FindResource("cubeSeed");
            var sceneSize = win.CurrentArt.SceneSize;
            var cubea = (double)win.FindResource("cubeA");
            var framea = sceneSize.Item1 * cubea;

            #region Draw outter frame
            // Draw a outter frame
            // Up
            mView.Children.Add(new GridLinesVisual3D()
            {
                Normal = new Vector3D(0, 1, 0),
                MajorDistance = framea,
                MinorDistance = framea,
                Center = new Point3D(framea / 2, framea, framea / 2),
                //Center = new Point3D(0, framea, 0),
                Length = framea,
                LengthDirection = new Vector3D(1, 0, 0),
                Width = framea
            });
            // Bottom
            mView.Children.Add(new GridLinesVisual3D()
            {
                Normal = new Vector3D(0, 1, 0),
                MajorDistance = framea,
                MinorDistance = framea,
                Center = new Point3D(framea / 2, 0, framea / 2),
                //Center = new Point3D(0, framea, 0),
                Length = framea,
                LengthDirection = new Vector3D(1, 0, 0),
                Width = framea
            });
            // Front
            mView.Children.Add(new GridLinesVisual3D()
            {
                Normal = new Vector3D(0, 0, 1),
                MajorDistance = framea,
                MinorDistance = framea,
                Center = new Point3D(framea / 2, framea / 2, framea),
                //Center = new Point3D(0, framea / 2, framea / 2),
                Length = framea,
                LengthDirection = new Vector3D(1, 0, 0),
                Width = framea
            });
            // Back
            mView.Children.Add(new GridLinesVisual3D()
            {
                Normal = new Vector3D(0, 0, 1),
                MajorDistance = framea,
                MinorDistance = framea,
                Center = new Point3D(framea / 2, framea / 2, 0),
                //Center = new Point3D(0, framea / 2, -framea / 2),
                Length = framea,
                LengthDirection = new Vector3D(1, 0, 0),
                Width = framea
            });
            // Left
            mView.Children.Add(new GridLinesVisual3D()
            {
                Normal = new Vector3D(1, 0, 0),
                MajorDistance = framea,
                MinorDistance = framea,
                Center = new Point3D(0, framea / 2, framea / 2),
                //Center = new Point3D(-framea / 2, framea / 2, 0),
                Length = framea,
                LengthDirection = new Vector3D(0, 0, 1),
                Width = framea
            });
            // Right
            mView.Children.Add(new GridLinesVisual3D()
            {
                Normal = new Vector3D(1, 0, 0),
                MajorDistance = framea,
                MinorDistance = framea,
                Center = new Point3D(framea, framea / 2, framea / 2),
                //Center = new Point3D(framea / 2, framea / 2, 0),
                Length = framea,
                LengthDirection = new Vector3D(0, 0, 1),
                Width = framea
            });
            #endregion

            // Pre-create all models.
            cubeModels = new GeometryModel3D[sceneSize.Item1 * sceneSize.Item2 * sceneSize.Item3];
            NameScope.SetNameScope(win, new NameScope());
            for (int i = 0; i != sceneSize.Item1; i++)
            {
                for (int j = 0; j != sceneSize.Item2; j++)
                {
                    for (int k = 0; k != sceneSize.Item3; k++)
                    {
                        GeometryModel3D c = cubeseed.Clone();
                        cubeModels[TupleToIdx(i, j, k)] = c;
                        mWin.RegisterName(NameForCubeModel(i, j, k), c);
                        //c.Transform = new TranslateTransform3D(cubea * i - sceneSize.Item1, cubea * j, cubea * k - sceneSize.Item1);
                        c.Transform = new TranslateTransform3D(cubea * i, cubea * j, cubea * k);
                    }
                }
            }

            // Show cubes on screen
            mCubeGroup.Children.Clear();
            for (int i = 0; i != sceneSize.Item1; i++)
                for (int j = 0; j != sceneSize.Item2; j++)
                    for (int k = 0; k != sceneSize.Item3; k++)
                    {
                        var c = CubeFromIdx(i, j, k);
                        var m = ModelFromIdx(i, j, k);
                        if (c.Visible)
                        {
                            m.Material = new MaterialGroup();
                            (m.Material as MaterialGroup).Children.Add(
                                new DiffuseMaterial(new SolidColorBrush(c.CubeColor)));

                            //添加发光材质
                            Color s = new Color();
                            s.ScA = (float)0.18824; s.ScB = 1; s.ScG = 1; s.ScR = 1;
                            (m.Material as MaterialGroup).Children.Add(     
                                new EmissiveMaterial(new SolidColorBrush(s)));

                            mCubeGroup.Children.Add(m);
                        }
                    }
        }

        Tuple<int, int, int> preFocus = null;
        public void SetFocus(int i, int j, int k)
        {
            // Clear previous focus
            if (preFocus != null)
            {
                GeometryModel3D preModel = ModelFromIdx(preFocus.Item1, preFocus.Item2, preFocus.Item3);
                var g = preModel.Material as MaterialGroup;
                var focusmaterial = g.Children.OfType<DiffuseMaterial>().LastOrDefault();
                g.Children.Remove(focusmaterial);

                var cube = CubeFromIdx(preFocus.Item1, preFocus.Item2, preFocus.Item3);
                if (!cube.Visible)
                {
                    mCubeGroup.Children.Remove(preModel);
                }
                
                preFocus = null;
            }
            // Set focus
            if (!(i < 0 || j < 0 || k < 0))
            {
                GeometryModel3D model = ModelFromIdx(i, j, k);
                preFocus = Tuple.Create(i, j, k);
                var g = model.Material as MaterialGroup;
                g.Children.Add(mWin.FindResource("focusMaterial") as Material);

                var cube = CubeFromIdx(preFocus.Item1, preFocus.Item2, preFocus.Item3);
                if (!cube.Visible)
                {
                    mCubeGroup.Children.Add(model);
                }
            }
        }

        public void Erase(int i, int j, int k)
        {
            var cube = CubeFromIdx(i, j, k);
            if (!cube.Visible)
                return;

            var model = ModelFromIdx(i, j, k);
            var g = model.Material as MaterialGroup;
            var old = g.Children.OfType<DiffuseMaterial>().First();
            g.Children.Remove(old);
            var n = mWin.FindResource("whiteSmokeMaterial") as Material;
            g.Children.Insert(0, n);

            cube.CubeColor = (Color) mWin.FindResource("whiteSmokeColor");
            cube.Visible = false;
            mCubeGroup.Children.Remove(model);
        }
        
        public void SetColor(int i, int j, int k, Color c)
        {
            // FIXME: only debug propose here!! Should delete this line in release.
            Color s = new Color();
            s.ScA = 1; s.ScB = (float)0.9882; s.ScG = (float)0.6863; s.ScR = (float)0.3765;
            c = s;
            // ENDFIXME

            var model = ModelFromIdx(i, j, k);
            var g = model.Material as MaterialGroup;
            var old = g.Children.OfType<DiffuseMaterial>().First();
            g.Children.Remove(old);
            var n = new DiffuseMaterial(new SolidColorBrush(c));
            g.Children.Insert(0, n);

            var cube = CubeFromIdx(i, j, k);
            if (!cube.Visible)
            {
                cube.Visible = true;
                mCubeGroup.Children.Add(model);
            }
            cube.CubeColor = c;
        }
        #endregion

        private Model3DGroup mCubeGroup;
        private HelixViewport3D mView;
        private MainWindow mWin;
        private GeometryModel3D[] cubeModels;

        public CubeSceneController()
        {
            
        }

        #region Convert i,j,k to index
        private int TupleToIdx(int i, int j, int k)
        {
            var sceneSize = mWin.CurrentArt.SceneSize;
            if (i >= 0 && j >= 0 && k >= 0
                && i <= sceneSize.Item1
                && j <= sceneSize.Item2
                && k <= sceneSize.Item3)
            {
                return i + j * sceneSize.Item1 + k * sceneSize.Item1 * sceneSize.Item2;
            }
            else
                throw new ArgumentOutOfRangeException("(i, j, k)", new Vector3D(i, j, k), "Shoule be >=0 && < SceneSize.Item1/Y/Z");
        }

        private int TupleToIdx(Tuple<int, int, int> tuple)
        {
            return TupleToIdx(tuple.Item1, tuple.Item2, tuple.Item3);
        }

        private int TupleToIdx(Vector3D tuple)
        {
            return TupleToIdx((int)tuple.X, (int)tuple.Y, (int)tuple.Z);
        }

        private GeometryModel3D ModelFromIdx(int i, int j, int k)
        {
            return cubeModels[TupleToIdx(i, j, k)];
        }

        private ICube CubeFromIdx(int i, int j, int k)
        {
            return mWin.CurrentArt.Cubes[TupleToIdx(i, j, k)];
        }
        #endregion

        private String NameForCubeModel(int i, int j, int k)
        {
            return String.Format("CubeModel{0}_{1}_{2}", i, j, k);
        }
    }
}
