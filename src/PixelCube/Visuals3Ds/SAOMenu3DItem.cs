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

        #region public Brush Foreground;
        /// <summary>
        /// Identifies the <see cref="Foreground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "Foreground",
            typeof(Brush),
            typeof(SAOMenu3DItem),
            new UIPropertyMetadata(default(Brush), ForegroundChanged));

        protected static void ForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3DItem).OnForegroundChanged(e);
        }

        protected virtual void OnForegroundChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null && Textboard != null)
                Textboard.Foreground = (Brush)e.NewValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public Brush Foreground
        {
            get { return (Brush)this.GetValue(ForegroundProperty); }
            set { this.SetValue(ForegroundProperty, value); }
        }
        #endregion

        #region public FontFamily FontFamily;
        /// <summary>
        /// Identifies the <see cref="FontFamily"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(
            "FontFamily",
            typeof(FontFamily),
            typeof(SAOMenu3DItem),
            new UIPropertyMetadata(default(FontFamily), FontFamilyChanged));

        protected static void FontFamilyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3DItem).OnFontFamilyChanged(e);
        }

        protected virtual void OnFontFamilyChanged(DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue != null && Textboard != null)
                Textboard.FontFamily = (FontFamily)e.NewValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public FontFamily FontFamily
        {
            get { return (FontFamily)this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }
        #endregion

        #region public bool IsVertical;
        /// <summary>
        /// Identifies the <see cref="IsVertical"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsVerticalProperty = DependencyProperty.Register(
            "IsVertical",
            typeof(bool),
            typeof(SAOMenu3DItem),
            new UIPropertyMetadata(false, IsVerticalChanged));

        protected static void IsVerticalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3DItem).OnIsVerticalChanged();
        }

        protected virtual void OnIsVerticalChanged()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsVertical
        {
            get { return (bool)this.GetValue(IsVerticalProperty); }
            set { this.SetValue(IsVerticalProperty, value); }
        }
        #endregion

        #region public SAOMenu3D parent;
        /// <summary>
        /// Identifies the <see cref="parent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty parentProperty = DependencyProperty.Register(
            "parent",
            typeof(SAOMenu3D),
            typeof(SAOMenu3DItem),
            new UIPropertyMetadata(default(SAOMenu3D), parentChanged));

        protected static void parentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3DItem).OnparentChanged();
        }

        protected virtual void OnparentChanged()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public SAOMenu3D parent
        {
            get { return (SAOMenu3D)this.GetValue(parentProperty); }
            set { this.SetValue(parentProperty, value); }
        }
        #endregion

        #region public bool IsSubMenu;
        /// <summary>
        /// Identifies the <see cref="IsSubMenu"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSubMenuProperty = DependencyProperty.Register(
            "IsSubMenu",
            typeof(bool),
            typeof(SAOMenu3DItem),
            new UIPropertyMetadata(default(bool), IsSubMenuChanged));

        protected static void IsSubMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SAOMenu3DItem).OnIsSubMenuChanged();
        }

        protected virtual void OnIsSubMenuChanged()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSubMenu
        {
            get { return (bool)this.GetValue(IsSubMenuProperty); }
            set { this.SetValue(IsSubMenuProperty, value); }
        }
        #endregion

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
            if(IsSubMenu)
            {
                parent.onsubmenu = true;
            }
            this.RaiseEvent(args);
            if (IsSubMenu)
            {
                parent.onsubmenu = false;
            }
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
