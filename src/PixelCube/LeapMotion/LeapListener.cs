﻿using System;
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
        public event EventHandler<DeviceInfoArgs> LeapStatusEvent;
        public event EventHandler<Vector> PreDrawOperationEvent;
        public event EventHandler<Vector> PreFocusOperationEvent;
        public event EventHandler<EventArgs> PreRotateOperationEvent;
        public event EventHandler<float> PreScaleOperationEvent;

    /* Attributes */
        private int pointableID;
        private bool isDrawing;
        private Frame lastFrame;
        private Frame currentFrame;
        
        
        /// <summary>
        ///     Initialize the private attributes
        /// </summary>
        /// <param name="controller"> Leap Controller </param>
        public override void OnInit(Controller controller)
        {
            pointableID = -1;
            isDrawing = false;
            lastFrame = null;
            currentFrame = null;
            
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
            EventHandler<DeviceInfoArgs> leapStatusEvent = LeapStatusEvent;
            //controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
            //controller.EnableGesture(Gesture.GestureType.TYPESWIPE);

            if (leapStatusEvent != null)
            {
                DeviceInfoArgs deviceInfoArg = new DeviceInfoArgs();
                deviceInfoArg.isConnected = true;
                leapStatusEvent(this, deviceInfoArg);
            }

            base.OnConnect(controller);

        }

        /// <summary>
        ///     Used to inform disconnect event
        /// </summary>
        /// <param name="controller"> Leap Controller </param>
        public override void OnDisconnect(Controller controller)
        {
            EventHandler<DeviceInfoArgs> leapStatusEvent = LeapStatusEvent;
            if (leapStatusEvent != null)
            {
                DeviceInfoArgs deviceInfoArg = new DeviceInfoArgs();
                deviceInfoArg.isConnected = false;
                leapStatusEvent(this, deviceInfoArg);
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
            currentFrame = controller.Frame();

            // If has two hands, suppose it will has a scale operation soon.
            if (currentFrame.Hands.Count > 2)
            {
                EventHandler<float> scale = PreScaleOperationEvent;
                scale(this, currentFrame.ScaleFactor(lastFrame));
                
            }

            if (currentFrame.Hands.Count == 1)
            {
                Hand hand = currentFrame.Hands[0];


            }







            //PointableList pointableList = frame.Pointables;
            //if (!pointableList.IsEmpty)
            //{
            //    Pointable pointable = frame.Pointable(pointableID);
            //    if (!pointable.IsValid) // Pointable is invalid, track a new one
            //    {
            //        pointable = frame.Pointables[0];
            //        pointableID = pointable.Id;
            //    }

            //    GestureList gestureList = frame.Gestures();
            //    foreach (Gesture gesture in gestureList)
            //    {
            //        switch (gesture.Type)
            //        {
            //            case Gesture.GestureType.TYPESCREENTAP:
            //                //ScreenTapGesture screenTapGesture = (ScreenTapGesture)gesture;
            //                if (PreDrawOperationEvent != null)
            //                {
            //                    EventHandler<Vector> draw = PreDrawOperationEvent;
            //                    draw(this, ((ScreenTapGesture)gesture).Position);
            //                }
            //                break;

            //            case Gesture.GestureType.TYPEKEYTAP:
            //                // Enter or exit dying mode
            //               isDrawing = isDrawing ? false : true;
            //               break;

            //           // case Gesture.GestureType.TYPESWIPE:

            //           //    break;


            //        }
            //    }


            //    // move to a new position
            //    if (focus != null)
            //    {
            //        focus(this, pointable.TipPosition);
            //    }
                
            //}
            base.OnFrame(controller);
        }







    }
}
