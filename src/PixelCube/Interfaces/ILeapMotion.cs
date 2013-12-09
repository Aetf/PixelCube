using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PixelCube.LeapMotion
{
    public interface ILeapMotion
    {
        void Initialize();    // LeapMotion initialize
        void Uninitialize();
        void LinkEvent();
        event EventHandler<LeapStatusChangeEventArgs> LeapStatusChangeEvent;    // Device status change
        event EventHandler<PreDrawOperationEventArgs> PreDrawOperationEvent;    // Draw event
        event EventHandler<PreFocusOperationEventArgs> PreFocusOperationEvent;  // Focus event
        event EventHandler<PreRotateOperationEventArgs> PreRotateOperationEvent;// Rotate event
        event EventHandler<PreScaleOperationEventArgs> PreScaleOperationEvent;  // Scale event
        event EventHandler<PreDragOperationEventArgs> PreDragOperationEvent;    // Drag event
        event EventHandler<PreEraseOperationEventArgs> PreEraseOperationEvent;  // Erase event
        event EventHandler<PreChangeColorOperationEventArgs> PreChangeColorOperationEvent; // ChangeColor event

    }
}
