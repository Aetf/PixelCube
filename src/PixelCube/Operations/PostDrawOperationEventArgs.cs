using System;

namespace PixelCube.Operations
{
    /// <summary>
    /// 上色事件参数
    /// </summary>
    public class PostDrawOperationEventArgs : EventArgs
    {
        public int Index { private set; get; }

        public PostDrawOperationEventArgs(int index)
        {
            this.Index = Index;
        }
    }
}
