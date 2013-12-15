using System;

namespace PixelCube.LeapMotion
{
    class LeapMenu : ILeapTrace
    {
        private LeapListener listener; // The only one listener
        private ILeapMotion leap;

        /// <summary>
        /// Exhale menu event
        /// </summary>
        public event EventHandler<ExhaleMenuArgs> ExhaleMenuEvent
        {
            add
            {
                listener.ExhaleMenuEvent += value;
            }
            remove
            {
                listener.ExhaleMenuEvent -= value;
            }
        }

        /// <summary>
        /// Select menu item event
        /// </summary>
        public event EventHandler<SelectMenuArgs> SelectMenuEvent
        {
            add
            {
                listener.SelectMenuEvent += value;
            }
            remove
            {
                listener.SelectMenuEvent -= value;
            }
        }

        /// <summary>
        /// Trace event
        /// </summary>
        public event EventHandler<TraceMenuArgs> TraceEvent
        {
            add
            {
                listener.TraceEvent += value;
            }
            remove
            {
                listener.TraceEvent -= value;
            }
        }

        public LeapMenu(ILeapMotion leap)
        {
            this.leap = leap;
        }

        public void DoInit()
        {
            listener = ((LeapController)leap).GetListener();
        }

        public void Uninitialize()
        {

        }
    }
}
