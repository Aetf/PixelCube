﻿using System;


namespace PixelCube.LeapMotion
{
    public interface ILeapMotion
    {
        void Initialize();    // LeapMotion initialize
        void Uninitialize();

        /// <summary>
        /// 取消初始化过程，取消后必须重新实例化该对象。
        /// </summary>
        void CancelInit();

        //LeapListener GetListener();
        event EventHandler<LeapModeChangeEventArgs> LeapModeChangeEvent;    // Device status change
        event EventHandler<PreDrawOperationEventArgs> PreDrawOperationEvent;    // Draw event
        event EventHandler<PreFocusOperationEventArgs> PreFocusOperationEvent;  // Focus event
        event EventHandler<PreRotateOperationEventArgs> PreRotateOperationEvent;// Rotate event
        event EventHandler<PreScaleOperationEventArgs> PreScaleOperationEvent;  // Scale event
        event EventHandler<PreDragOperationEventArgs> PreDragOperationEvent;    // Drag event
        event EventHandler<PreEraseOperationEventArgs> PreEraseOperationEvent;  // Erase event
        event EventHandler<PreChangeColorOperationEventArgs> PreChangeColorOperationEvent; // ChangeColor event
    }
}
