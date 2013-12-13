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

        public MatrixTransform3D WorldTransform { get; private set; }

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
            mCubeGroup = win.getCubeGroup();
            
            var cubeseed = (GeometryModel3D)win.FindResource("cubeSeed");
            var sceneSize = win.CurrentArt.SceneSize;
            var cubea = (double)win.FindResource("cubeA");
            var framea = sceneSize.X * cubea;

            #region Draw outter frame
            // Draw a outter frame
            // Up
            mView.Children.Add(new GridLinesVisual3D()
            {
                Normal = new Vector3D(0, 1, 0),
                MajorDistance = cubea,
                MinorDistance = cubea,
                Center = new Point3D(framea / 2, framea, framea / 2),
                Length = framea,
                LengthDirection = new Vector3D(1, 0, 0),
                Width = framea
            });
            // Bottom
            mView.Children.Add(new GridLinesVisual3D()
            {
                Normal = new Vector3D(0, 1, 0),
                MajorDistance = cubea,
                MinorDistance = cubea,
                Center = new Point3D(framea / 2, 0, framea / 2),
                Length = framea,
                LengthDirection = new Vector3D(1, 0, 0),
                Width = framea
            });
            // Front
            mView.Children.Add(new GridLinesVisual3D()
            {
                Normal = new Vector3D(0, 0, 1),
                MajorDistance = cubea,
                MinorDistance = cubea,
                Center = new Point3D(framea / 2, framea / 2, framea),
                Length = framea,
                LengthDirection = new Vector3D(1, 0, 0),
                Width = framea
            });
            // Back
            mView.Children.Add(new GridLinesVisual3D()
            {
                Normal = new Vector3D(0, 0, 1),
                MajorDistance = cubea,
                MinorDistance = cubea,
                Center = new Point3D(framea / 2, framea / 2, 0),
                Length = framea,
                LengthDirection = new Vector3D(1, 0, 0),
                Width = framea
            });
            // Left
            mView.Children.Add(new GridLinesVisual3D()
            {
                Normal = new Vector3D(1, 0, 0),
                MajorDistance = cubea,
                MinorDistance = cubea,
                Center = new Point3D(0, framea / 2, framea / 2),
                Length = framea,
                LengthDirection = new Vector3D(0, 0, 1),
                Width = framea
            });
            // Right
            mView.Children.Add(new GridLinesVisual3D()
            {
                Normal = new Vector3D(1, 0, 0),
                MajorDistance = cubea,
                MinorDistance = cubea,
                Center = new Point3D(framea, framea / 2, framea / 2),
                Length = framea,
                LengthDirection = new Vector3D(0, 0, 1),
                Width = framea
            });
            #endregion

            // Pre-create all models.
            cubeModels = new GeometryModel3D[(int)(sceneSize.X * sceneSize.Y * sceneSize.Z)];
            NameScope.SetNameScope(win, new NameScope());
            for (int i = 0; i != sceneSize.X; i++)
            {
                for (int j = 0; j != sceneSize.Y; j++)
                {
                    for (int k = 0; k != sceneSize.Z; k++)
                    {
                        GeometryModel3D c = cubeseed.Clone();
                        cubeModels[TupleToIdx(i, j, k)] = c;
                        mWin.RegisterName(NameForCubeModel(i, j, k), c);
                        c.Transform = new TranslateTransform3D(cubea*i, cubea*j, cubea*k);
                    }
                }
            }

            // Show cubes on screen
            for (int i = 0; i != sceneSize.X; i++)
                for (int j = 0; j != sceneSize.Y; j++)
                    for (int k = 0; k != sceneSize.Z; k++)
                    {
                        var c = CubeFromIdx(i, j, k);
                        var m = ModelFromIdx(i, j, k);
                        if (c.Visible)
                        {
                            m.Material = new MaterialGroup();
                            (m.Material as MaterialGroup).Children.Add(
                                new DiffuseMaterial(new SolidColorBrush(c.CubeColor)));
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
                var focusmaterial = g.Children.OfType<EmissiveMaterial>().LastOrDefault();
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
            c = Colors.WhiteSmoke;

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
                && i <= sceneSize.X
                && j <= sceneSize.Y
                && k <= sceneSize.Z)
            {
                return (int) (i + j * sceneSize.X + k * sceneSize.X * sceneSize.Y);
            }
            else
                throw new ArgumentOutOfRangeException("(i, j, k)", new Vector3D(i, j, k), "Shoule be >=0 && < SceneSize.X/Y/Z");
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
