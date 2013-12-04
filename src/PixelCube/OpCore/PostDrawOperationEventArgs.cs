using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelCube.OpCore
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
