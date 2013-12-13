using System;
using Leap;
using PixelCube.Scene3D;
using PixelCube.ThreeDimensional;

namespace PixelCube.LeapMotion
{
    class LeapController:ILeapMotion
    {
        internal IArtwork artWork;
        private Controller controller;
        private LeapListener listener;
        private CoordinatesTrans trans;

        // Public Attributes
        //#region public bool Visible;
        ///// <summary>
        ///// Identifies the <see cref="Visible"/> dependency property.
        ///// </summary>
        //public static readonly DependencyProperty VisibleProperty = DependencyProperty.Register(
        //    "Visible",
        //    typeof(bool),
        //    typeof(SAOMenu3D),
        //    new UIPropertyMetadata(default(bool), VisibleChanged));

        //protected static void VisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as SAOMenu3D).OnVisibleChanged();
        //}

        //protected virtual void OnVisibleChanged()
        //{
        //    showsb.Begin();
        //}

        ///// <summary>
        ///// Get or set the visibility of the menu.
        ///// </summary>
        //public bool Visible
        //{
        //    get { return (bool)this.GetValue(VisibleProperty); }
        //    set { this.SetValue(VisibleProperty, value); }
        //}
        //#endregion


        public event EventHandler<LeapModeChangeEventArgs> LeapModeChangeEvent;
        public event EventHandler<PreDrawOperationEventArgs> PreDrawOperationEvent;
        public event EventHandler<PreFocusOperationEventArgs> PreFocusOperationEvent;
        public event EventHandler<PreRotateOperationEventArgs> PreRotateOperationEvent;
        public event EventHandler<PreScaleOperationEventArgs> PreScaleOperationEvent;
        public event EventHandler<PreDragOperationEventArgs> PreDragOperationEvent;
        public event EventHandler<PreEraseOperationEventArgs> PreEraseOperationEvent;
        public event EventHandler<PreChangeColorOperationEventArgs> PreChangeColorOperationEvent;
        /// <summary>
        ///     Constructor
        /// </summary>
        public LeapController(IArtwork artWork)
        {
            trans = new CoordinatesTrans(artWork);
            listener = new LeapListener(trans);
            controller = new Controller();
        }

        #region Init&Uninit
        /* Intializer and Unintializer */
        public void Initialize()
        {
            controller.AddListener(listener);
        }

        public void LinkEvent()
        {
            listener.PreChangeColorOperationEvent += PreChangeColorOperationEvent;
            listener.PreDragOperationEvent += PreDragOperationEvent;
            listener.PreDrawOperationEvent += PreDrawOperationEvent;
            listener.PreEraseOperationEvent += PreEraseOperationEvent;
            listener.PreFocusOperationEvent += PreFocusOperationEvent;
            listener.PreRotateOperationEvent += PreRotateOperationEvent;
            listener.PreScaleOperationEvent += PreScaleOperationEvent;
            listener.LeapModeChangeEvent += LeapModeChangeEvent;
        }

        public void Uninitialize()
        {
            controller.RemoveListener(listener);
            controller.Dispose();
        }
        
        #endregion
    }
}
