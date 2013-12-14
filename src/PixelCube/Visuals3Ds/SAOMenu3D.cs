using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using PixelCube.Utils;

namespace PixelCube.Wpf
{
    [ContentProperty("Items")]
    public class SAOMenu3D : ModelVisual3D
    {
        Storyboard showsb;
        internal bool animating = false;

        public SAOMenu3D()
        {
            Items = new SAOMenu3DItemCollection();
            SelectionChanged += (obj, e) => ChangeFocus(e.OldIndex, e.NewIndex);
            UpdateModels();
        }

        #region Geometry3D SymbolGeometry
        /// <summary>
        /// Identifies the <see cref="SymbolGeometry"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SymbolGeometryProperty = DependencyProperty.Register(
            "SymbolGeometry",
            typeof(Geometry3D),
            typeof(SAOMenu3D),
            new UIPropertyMetadata(default(GeometryModel3D), SymbolGeometryChanged));

        protected static void SymbolGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3D).OnSymbolGeometryChanged();
        }

        protected virtual void OnSymbolGeometryChanged()
        {

        }

        /// <summary>
        /// Get or set the symbol geometry of the menu.
        /// </summary>
        public Geometry3D SymbolGeometry
        {
            get { return (Geometry3D)this.GetValue(SymbolGeometryProperty); }
            set { this.SetValue(SymbolGeometryProperty, value); }
        }
        #endregion

        #region public Point3D Pointer
        /// <summary>
        /// Identifies the <see cref="Pointer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PointerProperty = DependencyProperty.Register(
            "Pointer",
            typeof(Point3D),
            typeof(SAOMenu3D),
            new UIPropertyMetadata(default(Point3D), PointerChanged));

        protected static void PointerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3D).JudgeFocus();
        }

        /// <summary>
        /// Get or set the pointer position of the menu.
        /// </summary>
        public Point3D Pointer
        {
            get { return (Point3D)this.GetValue(PointerProperty); }
            set { this.SetValue(PointerProperty, value); }
        }
        #endregion

        #region public Point3D Position;
        /// <summary>
        /// Identifies the <see cref="Position"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position",
            typeof(Point3D),
            typeof(SAOMenu3D),
            new UIPropertyMetadata(default(Point3D), PositionChanged));

        protected static void PositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3D).OnPositionChanged();
        }

        protected virtual void OnPositionChanged()
        {
            UpdateModels();
        }

        /// <summary>
        /// Get or set the top left position of the menu.
        /// </summary>
        public Point3D Position
        {
            get { return (Point3D) this.GetValue(PositionProperty); }
            set { this.SetValue(PositionProperty, value); }
        }
        #endregion

        #region public double Distance;
        /// <summary>
        /// Identifies the <see cref="Distance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DistanceProperty = DependencyProperty.Register(
            "Distance",
            typeof(double),
            typeof(SAOMenu3D),
            new UIPropertyMetadata(30.0, DistanceChanged));

        protected static void DistanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3D).OnDistanceChanged();
        }

        protected virtual void OnDistanceChanged()
        {
            if(AutoCalcPosition)
                CalcPinPoint();
        }

        /// <summary>
        /// Get or set the diatance to the camera. Used when AutoCalcPosition is true
        /// </summary>
        public double Distance
        {
            get { return (double)this.GetValue(DistanceProperty); }
            set { this.SetValue(DistanceProperty, value); }
        }
        #endregion

        #region public bool Visible;
        /// <summary>
        /// Identifies the <see cref="Visible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VisibleProperty = DependencyProperty.Register(
            "Visible",
            typeof(bool),
            typeof(SAOMenu3D),
            new UIPropertyMetadata(false, VisibleChanged));

        protected static void VisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3D).OnVisibleChanged(e);
        }

        protected virtual void OnVisibleChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            if ((bool)e.NewValue)
            {
                if (this.GetViewport3D() == null)
                    return;

                var passed = SetupCamera();
                if (AutoCalcPosition)
                    CalcPinPoint();
                foreach (var item in Items)
                {
                    this.Children.Add(item.Symbol);
                    this.Children.Add(item.Textboard);
                }

                animating = true;
                showsb.BeginTime = TimeSpan.FromSeconds(passed);
                showsb.Begin();
            }
            else
            {
                foreach (var item in Items)
                {
                    item.Symbol.Opacity = 0;
                    item.Textboard.Opacity = 0;
                }
                this.Children.Clear();
                RestoreCamera();
            }
        }

        /// <summary>
        /// Get or set the visibility of the menu.
        /// </summary>
        public bool Visible
        {
            get { return (bool) this.GetValue(VisibleProperty); }
            set { this.SetValue(VisibleProperty, value); }
        }
        #endregion

        #region public SAOMenu3DItemCollection Items;
        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items",
            typeof(SAOMenu3DItemCollection),
            typeof(SAOMenu3D),
            new UIPropertyMetadata(new SAOMenu3DItemCollection(), ItemsChanged));

        protected static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3D).OnItemsChanged(e);
        }

        protected virtual void OnItemsChanged(DependencyPropertyChangedEventArgs e)
        {
            Items.CollectionChanged += (obj, arg) => UpdateModels();
        }

        /// <summary>
        /// Get or set the collection of SAOMenu3DItems.
        /// </summary>
        public SAOMenu3DItemCollection Items
        {
            get { return (SAOMenu3DItemCollection)this.GetValue(ItemsProperty); }
            set { this.SetValue(ItemsProperty, value); }
        }
        #endregion

        #region public bool AutoCalcPosition;
        /// <summary>
        /// Identifies the <see cref="AutoCalcPosition"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AutoCalcPositionProperty = DependencyProperty.Register(
            "AutoCalcPosition",
            typeof(bool),
            typeof(SAOMenu3D),
            new UIPropertyMetadata(true, AutoCalcPositionChanged));

        protected static void AutoCalcPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3D).OnAutoCalcPositionChanged();
        }

        protected virtual void OnAutoCalcPositionChanged()
        {
            if (AutoCalcPosition)
                CalcPinPoint();
        }

        /// <summary>
        /// Get or set whether auto calculate menu position according to distance and camera position
        /// </summary>
        public bool AutoCalcPosition
        {
            get { return (bool)this.GetValue(AutoCalcPositionProperty); }
            set { this.SetValue(AutoCalcPositionProperty, value); }
        }
        #endregion

        #region public double ScaleFactor;
        /// <summary>
        /// Identifies the <see cref="ScaleFactor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleFactorProperty = DependencyProperty.Register(
            "ScaleFactor",
            typeof(double),
            typeof(SAOMenu3D),
            new UIPropertyMetadata(2.0, ScaleFactorChanged));

        protected static void ScaleFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3D).OnScaleFactorChanged();
        }

        protected virtual void OnScaleFactorChanged()
        {
            if (Items.Count != 0)
                UpdateModels();
        }

        /// <summary>
        /// 
        /// </summary>
        public double ScaleFactor
        {
            get { return (double)this.GetValue(ScaleFactorProperty); }
            set { this.SetValue(ScaleFactorProperty, value); }
        }
        #endregion

        #region public double ZoomOutDistance;
        /// <summary>
        /// Identifies the <see cref="ZoomOutDistance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomOutDistanceProperty = DependencyProperty.Register(
            "ZoomOutDistance",
            typeof(double),
            typeof(SAOMenu3D),
            new UIPropertyMetadata(default(double), ZoomOutDistanceChanged));

        protected static void ZoomOutDistanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3D).OnZoomOutDistanceChanged();
        }

        protected virtual void OnZoomOutDistanceChanged()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public double ZoomOutDistance
        {
            get { return (double)this.GetValue(ZoomOutDistanceProperty); }
            set { this.SetValue(ZoomOutDistanceProperty, value); }
        }
        #endregion

        #region public double ActualHeight;
        /// <summary>
        /// Identifies the <see cref="ActualHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActualHeightProperty = DependencyProperty.Register(
            "ActualHeight",
            typeof(double),
            typeof(SAOMenu3D),
            new UIPropertyMetadata(default(double), ActualHeightChanged));

        protected static void ActualHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3D).OnActualHeightChanged();
        }

        protected virtual void OnActualHeightChanged()
        {
            if (AutoCalcPosition)
                CalcPinPoint();
        }

        /// <summary>
        /// 
        /// </summary>
        public double ActualHeight
        {
            get { return (double)this.GetValue(ActualHeightProperty); }
            private set { this.SetValue(ActualHeightProperty, value); }
        }
        #endregion

        #region public double ActualWidth;
        /// <summary>
        /// Identifies the <see cref="ActualWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActualWidthProperty = DependencyProperty.Register(
            "ActualWidth",
            typeof(double),
            typeof(SAOMenu3D),
            new UIPropertyMetadata(default(double), ActualWidthChanged));

        protected static void ActualWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3D).OnActualWidthChanged();
        }

        protected virtual void OnActualWidthChanged()
        {
            if (AutoCalcPosition)
                CalcPinPoint();
        }

        /// <summary>
        /// 
        /// </summary>
        public double ActualWidth
        {
            get { return (double)this.GetValue(ActualWidthProperty); }
            private set { this.SetValue(ActualWidthProperty, value); }
        }
        #endregion

        /// <summary>
        /// Toggle this menu.
        /// </summary>
        public void Toggle()
        {
            this.Visible = !this.Visible;
        }

        /// <summary>
        /// Show menu
        /// </summary>
        public void Show()
        {
            this.Visible = true;
        }

        /// <summary>
        /// Cause the current selected menu item to be activated.
        /// </summary>
        public void EnterCurrent()
        {
            if (animating)
                return;
            if(SelectedIndex >= 0 && SelectedIndex < Items.Count)
            {
                //Items[SelectedIndex].RaiseSelectedEvent(null);
                this.Visible = false;
                Items[SelectedIndex].RaiseSelectedEvent();
            }
        }

        #region Camera manipulation
        /// <summary>
        /// Zoom out the camera, so that menu will not be hiden by other objects
        /// </summary>
        /// <returns>
        /// time used to animate, in second
        /// </returns>
        private double SetupCamera()
        {
            if (this.GetViewport3D() == null || !(this.GetViewport3D().Camera is ProjectionCamera))
                return 0;

            var camera = this.GetViewport3D().Camera as ProjectionCamera;

            // Calculate directions
            var lookdir = camera.LookDirection; lookdir.Normalize();
            var outoffset = Vector3D.Multiply(-ZoomOutDistance, lookdir);
            var dest = Point3D.Add(camera.Position, outoffset);
            origCameraPos = camera.Position;
            var animationTime = 0.7;
            HelixToolkit.Wpf.CameraHelper.AnimateTo(camera, dest, camera.LookDirection, camera.UpDirection, animationTime * 1000);
            return animationTime;
        }
        Point3D origCameraPos;
        private void RestoreCamera()
        {
            if (this.GetViewport3D() == null || !(this.GetViewport3D().Camera is ProjectionCamera))
                return;

            var camera = this.GetViewport3D().Camera as ProjectionCamera;
            HelixToolkit.Wpf.CameraHelper.AnimateTo(camera, origCameraPos, camera.LookDirection, camera.UpDirection, 700);
        }

        #endregion

        protected virtual void JudgeFocus()
        {
            if (Items.Count == 0 || !Visible || animating)
                return;

            // Calculate three directions
            var lookdir = new Vector3D(0, 0, -1);
            var updir = new Vector3D(0, 1, 0);
            if (this.GetViewport3D() != null)
            {
                lookdir = this.GetViewport3D().Camera.GetLookDirection(); lookdir.Normalize();
                updir = this.GetViewport3D().Camera.GetUpDirection(); updir.Normalize();
            }
            var rightdir = Vector3D.CrossProduct(lookdir, updir); rightdir.Normalize();

            // yvec's projection on updir
            var y = distOn(Pointer, Position, updir);
            // A little scale for better operation.
            // Because the camera has been zoomed out but the world transform was not updated.
            double scale = distOn(origCameraPos, new Point3D(0, 0, 0), lookdir);
            scale = 1 + ZoomOutDistance / scale;
            y /= scale * 6;
            y += 2.5;
            System.Diagnostics.Debug.WriteLine("yafter = " + y);

            for(int i = 0; i!= Items.Count; i++)
            {
                var item = Items[i];
                // FIXME: threshold should be calc according to updir.
                var threshold = item.Symbol.Geometry.Bounds.SizeY;
                var lowbound = distOn(item.Symbol.Position, Position, updir) - threshold / 2;
                var upperbound = lowbound + threshold;

                if(y >= lowbound && y <= upperbound)
                {
                    // Little tricky, the element in Items are actually displayed in reversed order.
                    SelectedIndex = Items.Count - i - 1;
                    break;
                }
            }
        }

        private void ChangeFocus(int from, int to)
        {
            if (from != -1)
            {
                Items[from].Symbol.DecroateTransform = Transform3D.Identity;
            }
            Items[to].Symbol.DecroateTransform = new ScaleTransform3D(new Vector3D(1.5, 1.5, 1.5), Items[to].Symbol.Position);
        }

        private void UpdateModels()
        {
            if (SymbolGeometry == null)
                return;

            // Clear previous models.
            this.Children.Clear();
            // Create and re-add models.
            showsb = new Storyboard();
            showsb.FillBehavior = FillBehavior.Stop;
            showsb.Completed += (o, e) => this.animating = false;
            // Calculate three directions
            var lookdir = new Vector3D(0, 0, -1);
            var updir = new Vector3D(0, 1, 0);
            if (this.GetViewport3D() != null && this.GetViewport3D().Camera != null)
            {
                lookdir = this.GetViewport3D().Camera.GetLookDirection(); lookdir.Normalize();
                updir = this.GetViewport3D().Camera.GetUpDirection(); updir.Normalize();
            }
            var rightdir = Vector3D.CrossProduct(lookdir, updir); rightdir.Normalize();

            double width = 0;
            double height = 0;
            // Create models and apply animations
            for (int i = 0; i != Items.Count; i++)
            {
                double w = 0;
                var pos = Position;
                pos = Point3D.Add(Position, Vector3D.Multiply(updir, (Items.Count - i) * ScaleFactor * SymbolGeometry.Bounds.SizeY));
                // Symbol
                Items[i].Symbol = new SAOMenu3DSymbolVisual3D()
                {
                    Geometry = SymbolGeometry,
                    Color = Colors.Gray,
                    Position = pos,
                    Opacity = 0
                };
                w += Items[i].Symbol.Geometry.Bounds.SizeX * ScaleFactor;
                height += Items[i].Symbol.Geometry.Bounds.SizeY * ScaleFactor;

                // Textboard
                pos = Point3D.Add(pos, Vector3D.Multiply(rightdir, SymbolGeometry.Bounds.SizeX + 1));
                Items[i].Textboard = new SAOMenu3DTextBillboard()
                {
                    Text = Items[i].Header,
                    FontSize = 32,
                    Color = Colors.Red,
                    Opacity = 0,
                    Position = pos
                };
                w += Items[i].Textboard.Width + SymbolGeometry.Bounds.SizeX + 1;

                width = Math.Max(w, width);

                Items[i].showupanim = SetupShowTL(Items[i]);
                Items[i].showupanim.BeginTime = TimeSpan.FromSeconds(0.1 * (Items.Count - i - 1));
                showsb.Children.Add(Items[i].showupanim);
            }
            // Must update ActualWidth and ActualHeight once, or StackOverFlow
            ActualHeight = height;
            ActualWidth = width;
        }

        private ParallelTimeline SetupShowTL(SAOMenu3DItem item)
        {
            var sym = item.Symbol;
            var bd = item.Textboard;
            // Calculate three directions
            var lookdir = new Vector3D(0, 0, -1);
            var updir = new Vector3D(0, 1, 0);
            if (this.GetViewport3D() != null)
            {
                lookdir = this.GetViewport3D().Camera.GetLookDirection(); lookdir.Normalize();
                updir = this.GetViewport3D().Camera.GetUpDirection(); updir.Normalize();
            }
            var rightdir = Vector3D.CrossProduct(lookdir, updir); rightdir.Normalize();
            // Apply animations
            var tl = new ParallelTimeline();
            #region Opacity for sym
            var da = new DoubleAnimation();
            da.BeginTime = TimeSpan.FromSeconds(0);
            da.Duration = TimeSpan.FromSeconds(0.5);
            da.From = 0;
            da.To = 1;
            da.Completed += (sender, e) =>
            {
                sym.Opacity = 1;
            };
            Storyboard.SetTarget(da, sym);
            Storyboard.SetTargetProperty(da, new PropertyPath(SAOMenu3DSymbolVisual3D.OpacityProperty));
            tl.Children.Add(da);
            #endregion
            #region Position for sym
            var pa = new Point3DAnimation();
            pa.BeginTime = TimeSpan.FromSeconds(0);
            pa.Duration = TimeSpan.FromSeconds(0.2);
            pa.To = sym.Position;
            pa.From = Point3D.Add(sym.Position, Vector3D.Multiply(updir, 20));
            pa.EasingFunction = new QuadraticEase()
            {
                EasingMode = EasingMode.EaseOut
            };
            Storyboard.SetTarget(pa, sym);
            Storyboard.SetTargetProperty(pa, new PropertyPath(SAOMenu3DSymbolVisual3D.PositionProperty));
            tl.Children.Add(pa);
            #endregion
            #region Opacity for bd
            var da2 = new DoubleAnimation()
            {
                BeginTime = TimeSpan.FromSeconds(0),
                Duration = TimeSpan.FromSeconds(0.5),
                From = 0,
                To = 1
            };
            da2.Completed += (sender, e) =>
            {
                bd.Opacity = 1;
            };
            Storyboard.SetTarget(da2, sym);
            Storyboard.SetTargetProperty(da2, new PropertyPath(SAOMenu3DSymbolVisual3D.OpacityProperty));
            tl.Children.Add(da2);
            #endregion
            #region Position for bd
            var pa2 = new Point3DAnimation();
            pa2.BeginTime = TimeSpan.FromSeconds(0);
            pa2.Duration = TimeSpan.FromSeconds(0.2);
            pa2.To = bd.Position;
            pa2.From = Point3D.Add(bd.Position, Vector3D.Multiply(updir, 20));
            pa2.EasingFunction = new QuadraticEase()
            {
                EasingMode = EasingMode.EaseOut
            };
            Storyboard.SetTarget(pa2, bd);
            Storyboard.SetTargetProperty(pa2, new PropertyPath(HelixToolkit.Wpf.BillboardTextVisual3D.PositionProperty));
            tl.Children.Add(pa2);
            #endregion
            return tl;
        }

        private void CalcPinPoint()
        {
            Viewport3D parent = this.GetViewport3D();
            if(parent != null && parent.Camera != null)
            {
                var camera = parent.Camera;
                var lookoffset = camera.GetLookDirection();
                lookoffset.Normalize();
                lookoffset = Vector3D.Multiply(lookoffset, Distance);

                var downoffset = camera.GetUpDirection();
                downoffset.Normalize();
                downoffset = Vector3D.Multiply(downoffset, -ActualHeight / 2);
                
                var offset = Vector3D.Add(lookoffset, downoffset);
                
                Position = Point3D.Add(camera.GetPosition(), offset);
            }
        }

        private double distOn(Point3D point, Point3D orig, Vector3D dir)
        {
            var yvec = Point3D.Subtract(point, orig);
            // yvec's projection on updir
            return Vector3D.DotProduct(yvec, dir);
        }

        #region Selector Implements
        #region public int SelectedIndex;
        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex",
            typeof(int),
            typeof(SAOMenu3D),
            new UIPropertyMetadata(-1, SelectedIndexChanged));

        protected static void SelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3D).OnSelectedIndexChanged(e);
        }

        protected virtual void OnSelectedIndexChanged(DependencyPropertyChangedEventArgs e)
        {
            RaiseSelectionChanged((int) e.OldValue, (int) e.NewValue);
        }

        /// <summary>
        /// Get or set the pointer position of the menu.
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)this.GetValue(SelectedIndexProperty); }
            set { this.SetValue(SelectedIndexProperty, value); }
        }
        #endregion

        #region Event SelectionChanged
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
        private void RaiseSelectionChanged(int oldVal, int newVal)
        {
            if(SelectionChanged!=null)
            {
                SelectionChanged(this, new SelectionChangedEventArgs(oldVal, newVal));
            }
        }
        #endregion
        #endregion
    }

    public class SelectionChangedEventArgs : EventArgs
    {
        public int OldIndex { get; set; }
        public int NewIndex { get; set; }
        public SelectionChangedEventArgs(int oldval, int newval)
        {
            OldIndex = oldval;
            NewIndex = newval;
        }
    }
}
