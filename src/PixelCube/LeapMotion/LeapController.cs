using System;
using Leap;
using PixelCube.Scene3D;

namespace PixelCube.LeapMotion
{
    class LeapController:ILeapMotion
    {
        private Controller controller;
        private LeapListener listener;
        private CoordinatesTrans trans;

        public event EventHandler<LeapConnectionChangedEventArgs> LeapConnectionChangedEvent;
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

        public LeapListener GetListener() 
        {
            return listener;
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
            listener.LeapConntectionChangedEvent += LeapConnectionChangedEvent;
        }

        public void Uninitialize()
        {
            controller.RemoveListener(listener);
            controller.Dispose();
        }
        #endregion
    }
}
