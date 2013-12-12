using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace PixelCube.Scene3D
{
    class SerializeHepler
    {
        static List<ICube> outputList = new List<ICube>();
        static List<GeometryModel3D> inputList = new List<GeometryModel3D>();
        static GeometryModel3D cubeSeed;
        static int cubeNum;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cubeseed">立方体原型</param>
        /// <param name="cubenum">总立方体数(用于从空文件中初始化cube列表)</param>
        public SerializeHepler(GeometryModel3D cubeseed, int cubenum)
        {
            cubeSeed = cubeseed.Clone();
            cubeNum = cubenum;
        }

        #region 主要序列化操作
        /// <summary>
        /// 输出序列化列表
        /// </summary>
        /// <param name="cubeListIn3D">3D实例立方体列表</param>
        /// <returns></returns>
        public List<ICube> cubeOutput(List<GeometryModel3D> cubeListIn3D)
        {
            cubeListIn3D.ForEach(cubeOutAction);
            return outputList;
        }
        /// <summary>
        /// 输出列表的具体实现
        /// </summary>
        /// <param name="curCube"></param>
        private static void cubeOutAction(GeometryModel3D curCube)
        {
            cubeForSer tempCube = new cubeForSer(curCube.Material);
            outputList.Add(tempCube);
        }

        /// <summary>
        /// 读入列表
        /// </summary>
        /// <param name="cubeListInFile"></param>
        /// <returns>一维方块列表，顺序与绘制顺序一致</returns>
        public List<GeometryModel3D> cubeInput(List<ICube> cubeListInFile)
        {
            if (cubeListInFile.Count != 0)
                cubeListInFile.ForEach(cubeInAction);
            else
                for(int i = 0; i<cubeNum; i++)
                    inputList.Add(cubeSeed.Clone());
            return inputList;
        }

        /// <summary>
        /// 读入列表的具体实现
        /// </summary>
        /// <param name="curCube"></param>
        private static void cubeInAction(ICube curCube)
        {
            GeometryModel3D tempCube = cubeSeed.Clone();
            tempCube.Material = curCube.CubeMaterial;
        }
        #endregion
    }

    /// <summary>
    /// 序列化输出类
    /// </summary>
    class cubeForSer : ICube
    {
        public cubeForSer(Material m)
        {
            CubeMaterial = m;
        }

        Material CubeMaterial { get; set; }
    }
}
