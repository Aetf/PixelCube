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
      static internal EventHandler<LeapStatusChangeEventArgs> LeapStatusChangeEvent;
       internal EventHandler<PreDrawOperationEventArgs> PreDrawOperationEvent;
       internal EventHandler<PreFocusOperationEventArgs> PreFocusOperationEvent;
       internal EventHandler<PreRotateOperationEventArgs> PreRotateOperationEvent;
       internal EventHandler<PreScaleOperationEventArgs> PreScaleOperationEvent;
       internal EventHandler<PreDragOperationEventArgs> PreDragOperationEvent;
       internal EventHandler<PreEraseOperationEventArgs> PreEraseOperationEvent;
       internal EventHandler<PreChangeColorOperationEventArgs> PreChangeColorOperationEvent;
       #endregion   

       #region Attributes
       /* Attributes */
       private enum State
       {
           drawing,
           focusing,
           erasing,
           colorChanging,
           menuSelecting
       }


       private int pointableID;         // Used to track the pointable object
       private State state;             // Denote the current action(drawing, erasing, colorChanging, focusing)
       private Frame lastFrame;         // Record the lastFrame
       private Frame currentFrame;      // Represent the current frame
       private Controller controller;   // Refer to the Leap controller
       private CoordinatesTrans trans;
       #endregion

       public LeapListener(CoordinatesTrans trans)
       {
           this.trans = trans;
       }

        /// <summary>
        ///     Initialize the private attributes
        /// </summary>
        /// <param name="controller"> Leap Controller </param>
        public override void OnInit(Controller controller)
        {
            pointableID = -1;
            state = State.focusing;
            
            currentFrame = null;
            lastFrame = null;
            this.controller = controller;

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

          
            if (leapStatusChangeEvent != null)
            {
                Debug.WriteLine("LeapConnected");
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
            if (lastFrame == null)
                lastFrame = currentFrame;
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
                        Debug.WriteLine("ScaleEvent");
                        scale(this, new PreScaleOperationEventArgs(currentFrame.ScaleFactor(lastFrame)));
                    }
                }
                #endregion
                // Get left hand
                Hand h = currentFrame.Hands.Leftmost;
                
                // Rotate Action
               
                if (h.Fingers.Count > 4)
                {
                    #region Rotate
                    EventHandler<PreRotateOperationEventArgs> rotate = PreRotateOperationEvent;
                    if (rotate != null)
                    {
                        rotate(this, new PreRotateOperationEventArgs(h.RotationAxis(lastFrame),
                            h.RotationAngle(lastFrame)));
                    }
                    #endregion

                    // Drag Action
                    //#region Drag
                    //// One hand without any fingers is regarded as drag
                    //Vector transVector = h.Translation(lastFrame);
                    //if (trans.TransVector(transVector))
                    //{
                    //    EventHandler<PreDragOperationEventArgs> drag = PreDragOperationEvent;
                    //    if (drag != null)
                    //    {
                    //        drag(this, new PreDragOperationEventArgs(trans.getNewVec()));
                    //    }
                    //}
                    //#endregion
                }

            }

            // These two actions can be perfermed with only one hand or two hands
           

            #region Gestures
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
                            if(trans.TransPoint(screenTapGesture.Position))
                                draw(this, new PreDrawOperationEventArgs(trans.getNewVec()));
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
                        if (currentFrame.Fingers.Count >= 4)
                        {
                            state = State.erasing;
                            Debug.WriteLine("EraseMode");
                        }
                        break;
    
                }
            }
            #endregion

            PointableList pointables = currentFrame.Pointables;
            if (!pointables.IsEmpty)
            {
                Pointable pointable = currentFrame.Pointable(pointableID);
                if (!pointable.IsValid)  // Pointable is invalid, track a new one
                {
                    pointable = currentFrame.Pointables[0];
                    pointableID = pointable.Id;
                }

                // Ensure that this vector is valid
                if (trans.TransPoint(pointable.TipPosition))
                {
                    switch (state)
                    {
                        case State.focusing:
                            EventHandler<PreFocusOperationEventArgs> focus = PreFocusOperationEvent;
                            if (focus != null)
                            {
                                focus(this, new PreFocusOperationEventArgs(trans.getNewVec()));
                            }
                            break;

                        case State.erasing:
                            EventHandler<PreEraseOperationEventArgs> erase = PreEraseOperationEvent;
                            if (erase != null)
                            {
                                erase(this, new PreEraseOperationEventArgs(trans.getNewVec()));
                            }
                            break;

                        case State.drawing:
                            EventHandler<PreDrawOperationEventArgs> drawLine = PreDrawOperationEvent;
                            if (drawLine != null)
                            {
                                drawLine(this, new PreDrawOperationEventArgs(trans.getNewVec()));
                            }
                            break;

                        case State.colorChanging:
                            EventHandler<PreChangeColorOperationEventArgs> changeColor = PreChangeColorOperationEvent;
                            if (changeColor != null)
                            {
                                changeColor(this, new PreChangeColorOperationEventArgs(trans.getNewVec()));
                            }
                            break;
                    }
                }


            }

            // Store this frame
            lastFrame = currentFrame;
            base.OnFrame(controller);
        }







    }
}
