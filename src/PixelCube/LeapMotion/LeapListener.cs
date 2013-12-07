using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using System.Diagnostics;






namespace PixelCube.LeapMotion
{
    
    


    /// <summary>
    /// 
    /// </summary>
   internal class LeapListener:Listener
   {
       // Debug
              
       #region Events
       /// <summary>
       /// All events this class will offer
       /// </summary>
       internal EventHandler<LeapStatusChangeEventArgs> LeapStatusChangeEvent;
       internal event EventHandler<PreDrawOperationEventArgs> PreDrawOperationEvent;
       internal event EventHandler<PreFocusOperationEventArgs> PreFocusOperationEvent;
       internal event EventHandler<PreRotateOperationEventArgs> PreRotateOperationEvent;
       internal event EventHandler<PreScaleOperationEventArgs> PreScaleOperationEvent;
       internal event EventHandler<PreDragOperationEventArgs> PreDragOperationEvent;
       internal event EventHandler<PreEraseOperationEventArgs> PreEraseOperationEvent;
       internal event EventHandler<PreChangeColorOperationEventArgs> PreChangeColorOperationEvent;
       #endregion   

       #region Attributes
       /* Attributes */
       private enum State
       {
           drawing,
           focusing,
           erasing,
           colorChanging
       }


       private int pointableID;         // Used to track the pointable object
       private State state;             // Denote the current action(drawing, erasing, colorChanging, focusing)
       private Frame lastFrame;         // Record the lastFrame
       private Frame currentFrame;      // Represent the current frame
       private Controller controller;   // Refer to the Leap controller
       #endregion

        /// <summary>
        ///     Initialize the private attributes
        /// </summary>
        /// <param name="controller"> Leap Controller </param>
        public override void OnInit(Controller controller)
        {
            pointableID = -1;
            state = State.focusing;
            lastFrame = null;
            currentFrame = null;
            controller = null;
            
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
            EventHandler<LeapStatusChangeEventArgs> leapStatusChangeEvent = LeapStatusChangeEvent;
            //controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESWIPE);

            Debug.WriteLine("LeapConnected");
            if (leapStatusChangeEvent != null)
            {
                LeapStatusChangeEventArgs leapStatusChangeEventArgs = new LeapStatusChangeEventArgs();
                leapStatusChangeEventArgs.isConnected = true;
                leapStatusChangeEvent(this, leapStatusChangeEventArgs);
            }

            base.OnConnect(controller);

        }

        /// <summary>
        ///     Used to inform disconnect event
        /// </summary>
        /// <param name="controller"> Leap Controller </param>
        public override void OnDisconnect(Controller controller)
        {
            Debug.WriteLine("LeapDisconnected");
            EventHandler<LeapStatusChangeEventArgs> leapStatusChangeEvent = LeapStatusChangeEvent;
            if (leapStatusChangeEvent != null)
            {
                LeapStatusChangeEventArgs deviceInfoArg = new LeapStatusChangeEventArgs();
                deviceInfoArg.isConnected = false;
                leapStatusChangeEvent(this, deviceInfoArg);
            }
            base.OnDisconnect(controller);
        }

        /// <summary>
        ///     Prepared to be used..
        /// </summary>
        /// <param name="controller"> Controller this listener belongs to</param>
        public override void OnExit(Controller controller)
        {
            Debug.WriteLine("LeapExited");
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
            if (currentFrame.Hands.Count >= 2)
            {
                // Scale Action
                #region Scale
                // Suppose it is playing a scale action when the number of fingers greater than 7
                if (currentFrame.Fingers.Count >= 7)
                {
                    
                    EventHandler<PreScaleOperationEventArgs> scale = PreScaleOperationEvent;
                    if (scale != null)
                    {
                        scale(this, new PreScaleOperationEventArgs(currentFrame.ScaleFactor(lastFrame)));
                    }
                }
                #endregion

                // Rotate Action
                #region Rotate
                if (currentFrame.Fingers.Count > 4 && currentFrame.Fingers.Count < 7)
                {
                    EventHandler<PreRotateOperationEventArgs> rotate = PreRotateOperationEvent;
                    if (rotate != null)
                    {
                        rotate(this, new PreRotateOperationEventArgs(currentFrame.RotationAxis(lastFrame),
                            currentFrame.RotationAngle(lastFrame)));
                    }
                }
                
                #endregion

            }

            // Drag Action
            #region Drag
            if (currentFrame.Hands.Count > 0)

            {
                
                // One hand without any fingers is regarded as drag
                if (currentFrame.Fingers.Count < 1)
                {
                    Vector transVector = currentFrame.Translation(lastFrame);
                    EventHandler<PreDragOperationEventArgs> drag = PreDragOperationEvent;
                    if (drag != null)
                    {
                        drag(this, new PreDragOperationEventArgs(transVector));
                    }
                }
            }
            #endregion

            GestureList gestures = currentFrame.Gestures();
            foreach (Gesture gesture in gestures)
            {
                switch (gesture.Type)
                {
                    // draw one pixel
                    case Gesture.GestureType.TYPESCREENTAP:
                        EventHandler<PreDrawOperationEventArgs> draw = PreDrawOperationEvent;
                        if (draw != null)
                        {
                            ScreenTapGesture screenTapGesture = new ScreenTapGesture(gesture);
                            draw(this, new PreDrawOperationEventArgs(screenTapGesture.Position));
                        }
                        break;

                    // return to focus mode/draw mode
                    case Gesture.GestureType.TYPEKEYTAP:
                        if (state != State.focusing)
                        {
                            Debug.WriteLine("FocusMode");
                            state = State.focusing;
                        }
                        else
                        {
                            Debug.WriteLine("DrawMode");
                            state = State.drawing;
                        }
                        break;

                    // Enter erasing mode
                    case Gesture.GestureType.TYPESWIPE:
                        state = State.erasing;
                        Debug.WriteLine("EraseMode");
                        break;
    
                }
            }


            PointableList pointables = currentFrame.Pointables;
            if (!pointables.IsEmpty)
            {
                Pointable pointable = currentFrame.Pointable(pointableID);
                if (!pointable.IsValid)  // Pointable is invalid, track a new one
                {
                    pointable = currentFrame.Pointables[0];
                    pointableID = pointable.Id;
                }

                switch (state)
                {
                    case State.focusing:
                        EventHandler<PreFocusOperationEventArgs> focus = PreFocusOperationEvent;
                        if (focus != null)
                        {
                            focus(this, new PreFocusOperationEventArgs(pointable.TipPosition));
                        }
                        break;

                    case State.erasing:
                        EventHandler<PreEraseOperationEventArgs> erase = PreEraseOperationEvent;
                        if (erase != null)
                        {
                            erase(this, new PreEraseOperationEventArgs(pointable.TipPosition));
                        }
                        break;

                    case State.drawing:
                        EventHandler<PreDrawOperationEventArgs> drawLine = PreDrawOperationEvent;
                        if (drawLine != null)
                        {
                            drawLine(this, new PreDrawOperationEventArgs(pointable.TipPosition));
                        }
                        break;
                    
                    case State.colorChanging:
                        EventHandler<PreChangeColorOperationEventArgs> changeColor = PreChangeColorOperationEvent;
                        if (changeColor != null)
                        {
                            changeColor(this, new PreChangeColorOperationEventArgs(pointable.TipPosition));
                        }
                        break;
                }


            }

            // Store this frame
            lastFrame = currentFrame;
            base.OnFrame(controller);
        }







    }
}
