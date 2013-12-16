using System;
using System.Diagnostics;
using Leap;

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
       internal event EventHandler<LeapConnectionChangedEventArgs> LeapConntectionChangedEvent;
       internal event EventHandler<LeapModeChangeEventArgs> LeapModeChangeEvent;
       internal event EventHandler<PreDrawOperationEventArgs> PreDrawOperationEvent;
       internal event EventHandler<PreFocusOperationEventArgs> PreFocusOperationEvent;
       internal event EventHandler<PreRotateOperationEventArgs> PreRotateOperationEvent;
       internal event EventHandler<PreScaleOperationEventArgs> PreScaleOperationEvent;
       internal event EventHandler<PreDragOperationEventArgs> PreDragOperationEvent;
       internal event EventHandler<PreEraseOperationEventArgs> PreEraseOperationEvent;
       internal event EventHandler<PreChangeColorOperationEventArgs> PreChangeColorOperationEvent;

       // MenuEvents
       internal event EventHandler<ExhaleMenuArgs> ExhaleMenuEvent;    // Exhale Event
       internal event EventHandler<SelectMenuArgs> SelectMenuEvent;    // Select Event
       internal event EventHandler<TraceMenuArgs> TraceEvent;  // Trace Event
       #endregion   

       #region Private Attributes

       private int pointableID;         // Used to track the pointable object
       private State state;             // Denote the current action(drawing, erasing, colorChanging, focusing)
       private Frame lastFrame;         // Record the lastFrame
       private Frame currentFrame;      // Represent the current frame
       private Controller controller;   // Refer to the Leap controller
       private CoordinatesTrans trans;

       private Vector oriMenuPos;
      // private int menuCount;
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
            state = State.Normal;
            currentFrame = null;
            lastFrame = null;
            oriMenuPos = null;
            this.controller = controller;
            //menuCount = 0;

            // Waiting for connection
            while(!controller.IsConnected)
            {
                System.Threading.Thread.Sleep(20);
            }

            // Raise early stage event.
            if (LeapConntectionChangedEvent != null)
            {
                var leapStatusChangeEventArgs = new LeapConnectionChangedEventArgs();
                leapStatusChangeEventArgs.Connected = controller.IsConnected;
                LeapConntectionChangedEvent(this, leapStatusChangeEventArgs);
            }
            EventHandler<LeapModeChangeEventArgs> mode = LeapModeChangeEvent;
            if (mode != null)
            {
                mode(this, new LeapModeChangeEventArgs(state));
            }

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
            //controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESWIPE);

            if (LeapConntectionChangedEvent != null)
            {
                var leapStatusChangeEventArgs = new LeapConnectionChangedEventArgs();
                leapStatusChangeEventArgs.Connected = true;
                LeapConntectionChangedEvent(this, leapStatusChangeEventArgs);
            }
            EventHandler<LeapModeChangeEventArgs> mode = LeapModeChangeEvent;
            if (mode != null)
            {
                mode(this, new LeapModeChangeEventArgs(state));
            }

            Debug.WriteLine("connect");

            base.OnConnect(controller);
        }

        /// <summary>
        ///     Used to inform disconnect event
        /// </summary>
        /// <param name="controller"> Leap Controller </param>
        public override void OnDisconnect(Controller controller)
        {
            //Debug.WriteLine("LeapDisconnected");
            if (LeapConntectionChangedEvent != null)
            {
                var leapStatusChangeEventArgs = new LeapConnectionChangedEventArgs();
                leapStatusChangeEventArgs.Connected = false;
                LeapConntectionChangedEvent(this, leapStatusChangeEventArgs);
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
            //Pointable pointable;
            PointableList pointables = currentFrame.Pointables;
            Pointable pointable = currentFrame.Pointable(pointableID);

            #region Trace
            if (!pointables.IsEmpty)
            {
               //pointable = currentFrame.Pointable(pointableID);
                if (!pointable.IsValid)  // Pointable is invalid, track a new one
                {
                    pointable = currentFrame.Pointables[0];
                    pointableID = pointable.Id;
                }

                if (trans.CheckBound(pointable.TipPosition))
                {
                    if (TraceEvent != null)
                    {
                        TraceEvent(this, new TraceMenuArgs(trans.TransPoint(pointable.TipPosition)));
                    }
                }
                else // Invalid position
                {
                    base.OnFrame(controller);
                    return;
                }

            } else {
                base.OnFrame(controller);
                return;
            }
            #endregion

            // If has two hands, suppose it will has a scale operation soon.

            #region Menu

            #region MenuSelectingMode
            // menuSelectingMode doesn't need others action
            if (state == State.Menu)
            {
                 //The begining of the slide
                if (pointable.TipVelocity.Magnitude > 100 && pointable.TipVelocity.Magnitude < 800)
                {
                    oriMenuPos = pointable.TipPosition;
                    //menuCount = 0;
                }

                if (oriMenuPos != null
                    && pointable.TipPosition.x - oriMenuPos.x >80
                    //&& System.Math.Abs(pointable.TipPosition.z - oriMenuPos.z) < 60
                    && System.Math.Abs(oriMenuPos.y - pointable.TipPosition.y) < 30
                    && pointable.TipVelocity.Magnitude > 2000)
                {
                    //Debug.WriteLine("exhale menu");
                    //state = State.menuSelecting;
                    EventHandler<SelectMenuArgs> select = SelectMenuEvent;
                    if (select != null)
                    {
                        select(this, new SelectMenuArgs());
                        state = State.Normal;
                        EventHandler<LeapModeChangeEventArgs> modechange = LeapModeChangeEvent;
                        if (modechange != null)
                        {
                            modechange(this, new LeapModeChangeEventArgs(state));
                        }
                        base.OnFrame(controller);
                        return;
                    }

                }

                base.OnFrame(controller);
                return;

            }



            //if (state == State.Menu)
            //{

            //    if (currentFrame.Gestures().Count > 0)
            //    {
            //        GestureList menuGestures = currentFrame.Gestures();
            //        foreach (Gesture ges in menuGestures)
            //        {
            //            if (ges.Type == Gesture.GestureType.TYPESCREENTAP)
            //            {
            //                Debug.WriteLine("SelectMenu");
            //                EventHandler<SelectMenuArgs> select = SelectMenuEvent;
            //                if (select != null)
            //                {
            //                    select(this, new SelectMenuArgs());
            //                    state = State.Normal;
            //                    oriMenuPos = null;

            //                    EventHandler<LeapModeChangeEventArgs> modechange = LeapModeChangeEvent;
            //                    if (modechange != null)
            //                    {
            //                        modechange(this, new LeapModeChangeEventArgs(state));
            //                    }
            //                    base.OnFrame(controller);
            //                    return;
            //                }

            //            }
            //        }
            //    }


            //    base.OnFrame(controller);
            //    return;
            //}

            #endregion

            #region ExhaleMenu
            if (currentFrame.Hands.Count == 1 && currentFrame.Fingers.Count == 2 && state != State.Menu)
            {
                // The begin of the exhaling
                if (lastFrame.Hands.Count != 1 || lastFrame.Fingers.Count != 2)
                {
                    oriMenuPos = pointable.TipPosition;
                    //menuCount = 0;
                }

                if (oriMenuPos != null
                    && System.Math.Abs(pointable.TipPosition.x - oriMenuPos.x) < 30
                    //&& System.Math.Abs(pointable.TipPosition.z - oriMenuPos.z) < 60
                    && (oriMenuPos.y - pointable.TipPosition.y) > 80
                    && pointable.TipVelocity.Magnitude > 500)
                {
                    Debug.WriteLine("exhale menu");
                    //state = State.menuSelecting;
                    EventHandler<ExhaleMenuArgs> exhale = ExhaleMenuEvent;
                    if (exhale != null)
                    {
                        exhale(this, new ExhaleMenuArgs());
                        state = State.Menu;
                        EventHandler<LeapModeChangeEventArgs> modechange = LeapModeChangeEvent;
                        if (modechange != null)
                        {
                            modechange(this, new LeapModeChangeEventArgs(state));
                        }
                        base.OnFrame(controller);
                        return;
                    }
                    
                }
            }
            #endregion

            #endregion

            
            if (state != State.Menu)
            {
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
                                if (trans.CheckBound(screenTapGesture.Position))
                                {
                                    var tempPos = trans.TransPoint(screenTapGesture.Position);
                                    tempPos.Z += 8;
                                    draw(this, new PreDrawOperationEventArgs(tempPos));
                                }
                            }
                            break;

                        // return to focus mode/draw mode
                        case Gesture.GestureType.TYPEKEYTAP:

                            if (state != State.Normal)
                            {
                                state = State.Normal;
                            }
                            else
                            {
                                state = State.Drawing;
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
                                state = State.Erasing;
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

                #region TwoHands
                if (currentFrame.Hands.Count >= 2)
                {
                    // Scale Action
                    //#region Scale
                    //// Suppose it is playing a scale action when the number of fingers greater than 7
                    //if (currentFrame.Fingers.Count >= 9)
                    //{

                    //    EventHandler<PreScaleOperationEventArgs> scale = PreScaleOperationEvent;
                    //    if (scale != null)
                    //    {
                    //        scale(this, new PreScaleOperationEventArgs(currentFrame.ScaleFactor(lastFrame)));
                    //    }
                    //}
                    //#endregion
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
                #endregion

                #region OneHand
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
                            drag(this, new PreDragOperationEventArgs(trans.TransVector(transVector)));
                        }

                    }
                    #endregion
                }
                #endregion

                #region Pointable

                if (!pointables.IsEmpty)
                {
                    //pointable = currentFrame.Pointable(pointableID);
                    // Ensure that this vector is valid
                    if (trans.CheckBound(pointable.TipPosition))
                    {
                        var tipPosition = trans.TransPoint(pointable.TipPosition);
                        switch (state)
                        {
                            case State.Normal:
                                EventHandler<PreFocusOperationEventArgs> focus = PreFocusOperationEvent;
                                if (focus != null)
                                {
                                    focus(this, new PreFocusOperationEventArgs(tipPosition));
                                }
                                break;

                            case State.Erasing:
                                EventHandler<PreEraseOperationEventArgs> erase = PreEraseOperationEvent;
                                if (erase != null)
                                {
                                    erase(this, new PreEraseOperationEventArgs(tipPosition));
                                }

                                break;

                            case State.Drawing:
                                EventHandler<PreDrawOperationEventArgs> drawLine = PreDrawOperationEvent;
                                if (drawLine != null)
                                {
                                    drawLine(this, new PreDrawOperationEventArgs(tipPosition));
                                }

                                break;

                            case State.ChangingColor:
                                EventHandler<PreChangeColorOperationEventArgs> changeColor = PreChangeColorOperationEvent;
                                if (changeColor != null)
                                {
                                    changeColor(this, new PreChangeColorOperationEventArgs(tipPosition));
                                }
                                break;
                        }
                    }


                }
                #endregion

            }
            // Store this frame
            lastFrame = currentFrame;
            base.OnFrame(controller);
        }
    }
}
