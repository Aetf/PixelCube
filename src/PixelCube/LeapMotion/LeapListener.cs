using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;






namespace PixelCube.LeapMotion
{
    
    /// <summary>
    /// 
    /// </summary>
    class LeapListener:Listener,ILeapMotion
    {
        
        /// <summary>
        /// All events this class will offer
        /// </summary>
        public event EventHandler<DeviceInfoArgs> LeapStatusInfom;
        public event EventHandler<Vector> Dye;
        public event EventHandler<Vector> Move;
        public event EventHandler<EventArgs> Rotate;
        public event EventHandler<EventArgs> Zoom;

    /* Attributes */
        private int pointableID;
        private bool isDyeing;
        
        
        public override void OnInit(Controller controller)
        {
            pointableID = -1;
            isDyeing = false;
            base.OnInit(controller);
        }

        /// <summary>
        ///     Used to enable gestures and inform events
        /// </summary>
        /// <param name="controller">
        ///     LeapMotion controller
        /// </param>
        public override void OnConnect(Controller controller)
        {
            EventHandler<DeviceInfoArgs> leapStatusInfom = LeapStatusInfom;
            //controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESWIPE);

            if (leapStatusInfom != null)
            {
                DeviceInfoArgs deviceInfoArg = new DeviceInfoArgs();
                deviceInfoArg.isConnected = true;
                leapStatusInfom(this, deviceInfoArg);
            }

            base.OnConnect(controller);

        }

        /// <summary>
        ///     Used to inform disconnect event
        /// </summary>
        /// <param name="controller"> Leap Controller </param>
        public override void OnDisconnect(Controller controller)
        {
            EventHandler<DeviceInfoArgs> leapStatusInfom = LeapStatusInfom;
            if (leapStatusInfom != null)
            {
                DeviceInfoArgs deviceInfoArg = new DeviceInfoArgs();
                deviceInfoArg.isConnected = false;
                leapStatusInfom(this, deviceInfoArg);
            }
            base.OnDisconnect(controller);
        }

        /// <summary>
        ///     Prepared to be used..
        /// </summary>
        /// <param name="controller"> Controller this listener belongs to</param>
        public override void OnExit(Controller controller)
        {
            base.OnExit(controller);
        }

        /// <summary>
        ///     Every frame controller acquire
        /// </summary>
        /// <param name="controller"> Controller this listener belongs to</param>
        public override void OnFrame(Controller controller)
        {
            EventHandler<Vector> move = Move;
            Frame frame = controller.Frame();
            PointableList pointableList = frame.Pointables;
            if (!pointableList.IsEmpty)
            {
                Pointable pointable = frame.Pointable(pointableID);
                if (!pointable.IsValid) // Pointable is invalid, track a new one
                {
                    pointable = frame.Pointables[0];
                    pointableID = pointable.Id;
                }

                GestureList gestureList = frame.Gestures();
                foreach (Gesture gesture in gestureList)
                {
                    switch (gesture.Type)
                    {
                        case Gesture.GestureType.TYPESCREENTAP:
                            //ScreenTapGesture screenTapGesture = (ScreenTapGesture)gesture;
                            if (Dye != null)
                            {
                                EventHandler<Vector> dye = Dye;
                                dye(this, ((ScreenTapGesture)gesture).Position);
                            }
                            break;
                        case Gesture.GestureType.TYPEKEYTAP:
                            // Enter or exit dying mode
                           isDyeing = isDyeing ? false : true;
                           break;








                    }
                }


                // move to a new position
                if (move != null)
                {
                    move(this, pointable.TipPosition);
                }
                
            }
            base.OnFrame(controller);
        }







    }
}
