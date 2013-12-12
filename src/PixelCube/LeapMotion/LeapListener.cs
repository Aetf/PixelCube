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
      static internal EventHandler<LeapModeChangeEventArgs> LeapModeChangeEvent;
       internal EventHandler<PreDrawOperationEventArgs> PreDrawOperationEvent;
       internal EventHandler<PreFocusOperationEventArgs> PreFocusOperationEvent;
       internal EventHandler<PreRotateOperationEventArgs> PreRotateOperationEvent;
       internal EventHandler<PreScaleOperationEventArgs> PreScaleOperationEvent;
       internal EventHandler<PreDragOperationEventArgs> PreDragOperationEvent;
       internal EventHandler<PreEraseOperationEventArgs> PreEraseOperationEvent;
       internal EventHandler<PreChangeColorOperationEventArgs> PreChangeColorOperationEvent;
       #endregion   

       #region Private Attributes
       /* Attributes */
       //private enum State
       //{
       //    drawing,
       //    focusing,
       //    erasing,
       //    colorChanging,
       //    menuSelecting
       //}


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
            EventHandler<LeapModeChangeEventArgs> leapStatusChangeEvent = LeapModeChangeEvent;
            //controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESWIPE);

          
            //if (leapStatusChangeEvent != null)
            //{
            //    LeapModeChangeEventArgs leapStatusChangeEventArgs = new LeapModeChangeEventArgs();
            //    leapStatusChangeEventArgs.isConnected = true;
            //    leapStatusChangeEvent(this, leapStatusChangeEventArgs);
            //}

            base.OnConnect(controller);

        }

        /// <summary>
        ///     Used to inform disconnect event
        /// </summary>
        /// <param name="controller"> Leap Controller </param>
        public override void OnDisconnect(Controller controller)
        {
            //Debug.WriteLine("LeapDisconnected");
            //EventHandler<LeapModeChangeEventArgs> leapStatusChangeEvent = LeapModeChangeEvent;
            //if (leapStatusChangeEvent != null)
            //{
            //    LeapModeChangeEventArgs deviceInfoArg = new LeapModeChangeEventArgs();
            //    deviceInfoArg.isConnected = false;
            //    leapStatusChangeEvent(this, deviceInfoArg);
            //}
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
                if (currentFrame.Fingers.Count >= 9)
                {
                    
                    EventHandler<PreScaleOperationEventArgs> scale = PreScaleOperationEvent;
                    if (scale != null)
                    {
                        scale(this, new PreScaleOperationEventArgs(currentFrame.ScaleFactor(lastFrame)));
                    }
                }
                #endregion
                // Get left hand

                Hand leftHand = currentFrame.Hands.Leftmost;
                // Rotate Action

                if (leftHand.Fingers.Count > 3)
                {
                    #region Rotate
                    EventHandler<PreRotateOperationEventArgs> rotate = PreRotateOperationEvent;
                    if (rotate != null)
                    {
                        rotate(this, new PreRotateOperationEventArgs(leftHand.RotationAxis(lastFrame),
                            leftHand.RotationAngle(lastFrame)));
                    }
                    #endregion
                }
            }
           
            // Just one hand and above 4 fingers
            if (lastFrame.Hands.Count == 1 && currentFrame.Hands.Count == 1)
            {
                Hand hand = currentFrame.Hands[0];
                #region Drag
                if (hand.Fingers.Count > 3)
                {
                    
                    EventHandler<PreDragOperationEventArgs> drag = PreDragOperationEvent;
                    if (drag != null)
                    {
                        Vector transVector = hand.PalmPosition - lastFrame.Hands[0].PalmPosition;
                        //transVector.z = 0;
                        trans.TransVector(transVector);
                        drag(this, new PreDragOperationEventArgs(transVector));
                    }

                }
                #endregion
            }
            

            // These two actions can be perfermed with only one hand or two hands
            #region Gestures
            GestureList gestures = currentFrame.Gestures();
            foreach (Gesture gesture in gestures)
            {
//                HandList hands = gesture.Hands;
                
                switch (gesture.Type)
                {
                    // draw one pixel
                    case Gesture.GestureType.TYPESCREENTAP:
                        EventHandler<PreDrawOperationEventArgs> draw = PreDrawOperationEvent;
                        if (draw != null)
                        {
                            ScreenTapGesture screenTapGesture = new ScreenTapGesture(gesture);
                            if (trans.TransPoint(screenTapGesture.Position))
                            {
                                Vector tempVec = trans.getNewVec();
                                tempVec.z += 8;
                                draw(this, new PreDrawOperationEventArgs(tempVec));
                            }
                        }
                        break;

                    // return to focus mode/draw mode
                    case Gesture.GestureType.TYPEKEYTAP:
                       
                        if (state != State.focusing)
                        {
                            state = State.focusing;
                        }
                        else
                        {
                            state = State.drawing;
                        }
                    
                        EventHandler<LeapModeChangeEventArgs> mode = LeapModeChangeEvent;
                        if (mode != null)
                        {
                            mode(this, new LeapModeChangeEventArgs(state));
                        }

                        break;

                    // Enter erasing mode
                    case Gesture.GestureType.TYPESWIPE:
                        if (currentFrame.Fingers.Count >= 4)
                        {
                            state = State.erasing;
                        }
                        EventHandler<LeapModeChangeEventArgs> mode2 = LeapModeChangeEvent;
                        if (mode2 != null)
                        {
                            mode2(this, new LeapModeChangeEventArgs(state));
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
