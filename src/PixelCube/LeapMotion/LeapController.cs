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

        public event EventHandler<LeapConnectionChangedEventArgs> LeapConnectionChangedEvent
        {
            add
            {
                listener.LeapConntectionChangedEvent += value;
            }
            remove
            {
                listener.LeapConntectionChangedEvent -= value;
            }
        }
        public event EventHandler<LeapModeChangeEventArgs> LeapModeChangeEvent
        {
            add
            {
                listener.LeapModeChangeEvent += value;
            }
            remove
            {
                listener.LeapModeChangeEvent -= value;
            }
        }
        public event EventHandler<PreDrawOperationEventArgs> PreDrawOperationEvent
        {
            add
            {
                listener.PreDrawOperationEvent += value;
            }
            remove
            {
                listener.PreDrawOperationEvent -= value;
            }
        }
        public event EventHandler<PreFocusOperationEventArgs> PreFocusOperationEvent
        {
            add
            {
                listener.PreFocusOperationEvent += value;
            }
            remove
            {
                listener.PreFocusOperationEvent -= value;
            }
        }
        public event EventHandler<PreRotateOperationEventArgs> PreRotateOperationEvent
        {
            add
            {
                listener.PreRotateOperationEvent += value;
            }
            remove
            {
                listener.PreRotateOperationEvent -= value;
            }
        }
        public event EventHandler<PreScaleOperationEventArgs> PreScaleOperationEvent
        {
            add
            {
                listener.PreScaleOperationEvent += value;
            }
            remove
            {
                listener.PreScaleOperationEvent -= value;
            }
        }
        public event EventHandler<PreDragOperationEventArgs> PreDragOperationEvent
        {
            add
            {
                listener.PreDragOperationEvent += value;
            }
            remove
            {
                listener.PreDragOperationEvent -= value;
            }
        }
        public event EventHandler<PreEraseOperationEventArgs> PreEraseOperationEvent
        {
            add
            {
                listener.PreEraseOperationEvent += value;
            }
            remove
            {
                listener.PreEraseOperationEvent -= value;
            }
        }
        public event EventHandler<PreChangeColorOperationEventArgs> PreChangeColorOperationEvent
        {
            add
            {
                listener.PreChangeColorOperationEvent += value;
            }
            remove
            {
                listener.PreChangeColorOperationEvent -= value;
            }
        }
        /// <summary>
        ///     Constructor
        /// </summary>
        public LeapController(IArtwork artWork)
        {
            trans = new CoordinatesTrans(artWork);
            listener = new LeapListener(trans);
        }

        public LeapListener GetListener() 
        {
            return listener;
        }


        #region Init&Uninit
        /* Intializer and Unintializer */
        public void Initialize()
        {
            controller = new Controller(listener);
        }

        public void Uninitialize()
        {
            controller.RemoveListener(listener);
            controller.Dispose();
        }
        #endregion
    }
}
