using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelCube.LeapMotion
{
    public interface ILeapTrace
    {
        event EventHandler<ExhaleMenuArgs> ExhaleMenuEvent;    // Exhale Event
        event EventHandler<SelectMenuArgs> SelectMenuEvent;    // Select Event
        event EventHandler<TraceMenuArgs> TraceEvent;  // Trace Event
        void LinkEvent();
        void Initialize();    // LeapMotion initialize
        void Uninitialize();
    }
}
