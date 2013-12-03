﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using PixelCube.LeapMotion;

namespace PixelCube.OpCore
{
    /// <summary>
    /// 焦点变换事件参数
    /// </summary>
    public class PostFocusOperationEventArgs : EventArgs
    {
        public PostFocusOperationEventArgs(string car)
        {
            //this.Car = car;
        }
    }

    /// <summary>
    /// 上色事件参数
    /// </summary>
    public class PostDrawOperationEventArgs : EventArgs
    {
        public PostDrawOperationEventArgs()
        {
        }
    }

    /// <summary>
    /// 旋转事件参数
    /// </summary>
    public class PostRotateOperationEventArgs : EventArgs
    {
        public PostRotateOperationEventArgs()
        {
        }
    }

    /// <summary>
    /// 缩放事件参数
    /// </summary>
    public class PostScaleOperationEventArgs : EventArgs
    {
        public PostScaleOperationEventArgs()
        {
        }
    }

    /// <summary>
    /// 操作核心实现类
    /// </summary>
    public class OpCore
    {
        /// <summary>
        /// 焦点变化事件
        /// </summary>
        public event EventHandler<PostFocusOperationEventArgs> PostFocusOperationEvent;

        /// <summary>
        /// 小方块着色事件
        /// </summary>
        public event EventHandler<PostDrawOperationEventArgs> PostDrawOperationEvent;

        /// <summary>
        /// 世界角度变换事件
        /// </summary>
        public event EventHandler<PostRotateOperationEventArgs> PostRotateOperationEvent;

        /// <summary>
        /// 世界缩放事件
        /// </summary>
        public event EventHandler<PostScaleOperationEventArgs> PostScaleOperationEvent;

        public OpCore()
        {
        }

        /// <summary>
        /// 焦点变化响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnPreFocusOperation(object sender, PreFocusOperationEventArgs e)
        {

        }

        /// <summary>
        /// 旋转操作响应函数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void OnPreRotateOperation(object sender, PreRotateOperationEventArgs e)
        {
        }
    }
}
