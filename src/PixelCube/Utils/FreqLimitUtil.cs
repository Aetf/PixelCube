using System;
using System.Collections.Generic;

namespace PixelCube.Utils
{
    public static class FreqLimitUtil
    {
        static internal Dictionary<String, Object> dict = new Dictionary<String, Object>();

        /// <summary>
        /// 检查与此tag相关的stamp是否已经存在.
        /// 通常用法：用于判断是否已经在短时间内调用过与此tag相关的方法
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="stamp"></param>
        /// <returns>如果stamp已经存在返回true</returns>
        public static bool CheckFreq(String tag, Object stamp)
        {
            if (dict.ContainsKey(tag) && dict[tag].Equals(stamp))
                return true;

            dict[tag] = stamp;
            return false;
        }
    }
}
