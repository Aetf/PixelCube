using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using PixelCube.Utils;

namespace PixelCube.Wpf
{
    public class SAOMenu3DItem : UIElement3D
    {
        internal ParallelTimeline showupanim;
        internal SAOMenu3DSymbolVisual3D Symbol;
        internal SAOMenu3DTextBillboard Textboard;

        #region Selected Route Event
        /// <summary>
        /// The item selected event.
        /// </summary>
        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent(
            "Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SAOMenu3DItem));

        /// <summary>
        /// Event when this item has been selected
        /// </summary>
        public event RoutedEventHandler Selected
        {
            add
            {
                this.AddHandler(SelectedEvent, value);
            }

            remove
            {
                this.RemoveHandler(SelectedEvent, value);
            }
        }

        internal void RaiseSelectedEvent()
        {
            var args = new RoutedEventArgs(SelectedEvent, this);
            this.RaiseEvent(args);
        }
        #endregion

        #region public String Header;
        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof(String),
            typeof(SAOMenu3DItem),
            new UIPropertyMetadata(default(String), HeaderChanged));

        protected static void HeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3DItem).OnHeaderChanged(e);
        }

        protected virtual void OnHeaderChanged(DependencyPropertyChangedEventArgs e)
        {
            if(Textboard != null)
                Textboard.Text = (String)e.NewValue;
        }

        /// <summary>
        /// 获取和设置菜单文字
        /// </summary>
        public String Header
        {
            get { return (String)this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }
        #endregion
    }

    internal class SAOMenu3DTextBillboard : BillboardTextVisual3D
    {
        #region public double Opacity;
        /// <summary>
        /// Identifies the <see cref="Opacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register(
            "Opacity",
            typeof(double),
            typeof(SAOMenu3DTextBillboard),
            new UIPropertyMetadata(default(double), OpacityChanged));

        protected static void OpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3DTextBillboard).OnOpacityChanged(e);
        }

        private void OnOpacityChanged(DependencyPropertyChangedEventArgs e)
        {
            (Foreground as SolidColorBrush).Opacity = (double)e.NewValue;
        }

        /// <summary>
        /// Get or set the opacity of the text billboard
        /// </summary>
        public double Opacity
        {
            get { return (double)this.GetValue(OpacityProperty); }
            set { this.SetValue(OpacityProperty, value); }
        }
        #endregion

        #region public Color Color;
        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color",
            typeof(Color),
            typeof(SAOMenu3DTextBillboard),
            new UIPropertyMetadata(Colors.Black, ColorChanged));

        protected static void ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3DTextBillboard).OnColorChanged(e);
        }

        private void OnColorChanged(DependencyPropertyChangedEventArgs e)
        {
            (Foreground as SolidColorBrush).Color = (Color)e.NewValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public Color Color
        {
            get { return (Color)this.GetValue(ColorProperty); }
            set { this.SetValue(ColorProperty, value); }
        }
        #endregion

        public SAOMenu3DTextBillboard()
        {
            var b = new SolidColorBrush(Color);
            b.Opacity = Opacity;
            Foreground = b;
        }
    }

    internal class SAOMenu3DSymbolVisual3D : ModelVisual3D
    {
        #region public Point3D Position;
        /// <summary>
        /// Identifies the <see cref="Position"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position",
            typeof(Point3D),
            typeof(SAOMenu3DSymbolVisual3D),
            new UIPropertyMetadata(default(Point3D), PositionChanged));

        protected static void PositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3DSymbolVisual3D).OnPositionChanged(e);
        }

        private Transform3D posTrans;
        protected virtual void OnPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            posTrans = new TranslateTransform3D(Point3D.Subtract((Point3D)e.NewValue, default(Point3D)));
            UpdateTransform();
        }

        /// <summary>
        /// Get or set the position.
        /// </summary>
        public Point3D Position
        {
            get { return (Point3D)this.GetValue(PositionProperty); }
            set { this.SetValue(PositionProperty, value); }
        }
        #endregion

        #region public double Opacity;
        /// <summary>
        /// Identifies the <see cref="Opacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register(
            "Opacity",
            typeof(double),
            typeof(SAOMenu3DSymbolVisual3D),
            new UIPropertyMetadata(default(double), OpacityChanged));

        protected static void OpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3DSymbolVisual3D).OnOpacityChanged(e);
        }

        protected virtual void OnOpacityChanged(DependencyPropertyChangedEventArgs e)
        {
            var dm = (this.Content as GeometryModel3D).Material as DiffuseMaterial;
            dm.Brush.Opacity = (double)e.NewValue;
        }

        /// <summary>
        /// Get or set the top left position of the menu.
        /// </summary>
        public double Opacity
        {
            get { return (double)this.GetValue(OpacityProperty); }
            set { this.SetValue(OpacityProperty, value); }
        }
        #endregion

        #region public Color Color;
        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color",
            typeof(Color),
            typeof(SAOMenu3DSymbolVisual3D),
            new UIPropertyMetadata(default(Color), ColorChanged));

        protected static void ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3DSymbolVisual3D).OnColorChanged(e);
        }

        protected virtual void OnColorChanged(DependencyPropertyChangedEventArgs e)
        {
            var dm = (this.Content as GeometryModel3D).Material as DiffuseMaterial;
            var scb = dm.Brush as SolidColorBrush;
            scb.Color = (Color)e.NewValue;
        }

        /// <summary>
        /// Get or set the color of the menu.
        /// </summary>
        public Color Color
        {
            get { return (Color)this.GetValue(ColorProperty); }
            set { this.SetValue(ColorProperty, value); }
        }
        #endregion

        #region public Transform3D DecroateTransform;
        /// <summary>
        /// Identifies the <see cref="DecroateTransform"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DecroateTransformProperty = DependencyProperty.Register(
            "DecroateTransform",
            typeof(Transform3D),
            typeof(SAOMenu3DSymbolVisual3D),
            new UIPropertyMetadata(default(Transform3D), DecroateTransformChanged));

        protected static void DecroateTransformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3DSymbolVisual3D).OnDecroateTransformChanged(e);
        }

        protected virtual void OnDecroateTransformChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateTransform();
            //(this.Content as GeometryModel3D).Transform = geoOffset;
            //(this.Content as GeometryModel3D).AddTransform((Transform3D)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public Transform3D DecroateTransform
        {
            get { return (Transform3D)this.GetValue(DecroateTransformProperty); }
            set { this.SetValue(DecroateTransformProperty, value); }
        }
        #endregion

        public Geometry3D Geometry
        {
            get { return (this.Content as GeometryModel3D).Geometry; }
            set
            {
                (this.Content as GeometryModel3D).Geometry = value;
                GeometryExtension.MakeSureCenterZero(this.Content as GeometryModel3D);
            }
        }

        public SAOMenu3DSymbolVisual3D()
        {
            var brush = new SolidColorBrush(Color);
            brush.Opacity = Opacity;
            this.Content = new GeometryModel3D()
            {
                Material = new DiffuseMaterial(brush)
            };
        }

        private void UpdateTransform()
        {
            this.Transform = posTrans;
            this.AddTransform(this.DecroateTransform);
        }
    }

    public class SAOMenu3DItemCollection : ObservableCollection<SAOMenu3DItem>
    {

    }
}
