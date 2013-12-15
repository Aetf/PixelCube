using System;

namespace PixelCube.Operations
{
    /// <summary>
    /// 焦点变换事件参数
    /// </summary>
    public class PostFocusOperationEventArgs : EventArgs
    {
        public int Index { private set; get; }
        public PostFocusOperationEventArgs(int index)
        {
            this.Index = index;
        }
    }
}
