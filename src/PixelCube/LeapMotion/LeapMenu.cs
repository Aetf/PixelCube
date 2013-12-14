using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelCube.LeapMotion
{
    class LeapMenu:ILeapTrace
    {
        private LeapListener listener; // The only one listener
        private ILeapMotion leap;
        
       public event EventHandler<ExhaleMenuArgs> ExhaleMenuEvent;    // Exhale Event
       public event EventHandler<SelectMenuArgs> SelectMenuEvent;    // Select Event
       public event EventHandler<TraceMenuArgs> TraceEvent;  // Trace Event


       public LeapMenu(ILeapMotion leap)
       {
           this.leap = leap;
       }


       public void LinkEvent()
       {
           listener.ExhaleMenuEvent += ExhaleMenuEvent;
           listener.SelectMenuEvent += SelectMenuEvent;
           listener.TraceMenuEvent += TraceEvent;
       }
       public void Initialize()
       {
           listener = ((LeapController)leap).GetListener();
       }
       public void Uninitialize()
       {

       }
    }
}
