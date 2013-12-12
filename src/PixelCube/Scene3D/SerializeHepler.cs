using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using PixelCube.ThreeDimensional;

namespace PixelCube.Scene3D
{
    static class SerializeHepler
    {
        #region 主要序列化操作
        /// <summary>
        /// 输出序列化列表
        /// </summary>
        /// <param name="cubeListIn3D">3D实例立方体列表</param>
        /// <returns></returns>
        public static List<Cube> cubeOutput(GeometryModel3D[] cubeListIn3D)
        {
            List<Cube> cubeList = new List<Cube>();
            Array.ForEach(cubeListIn3D, new Action<GeometryModel3D>(cm =>
                {
                    Cube c = new Cube();
                    var group = cm.Material as MaterialGroup;
                    var brush = group.Children.OfType<DiffuseMaterial>().First().Brush as SolidColorBrush;
                    c.CubeColor = brush.Color;
                    cubeList.Add(c);
                }));
            return cubeList;
        }

        /// <summary>
        /// 读入列表
        /// </summary>
        /// <param name="seed">立方体原型</param>
        /// <param name="cubeListInFile"></param>
        /// <returns>一维方块列表，顺序与绘制顺序一致</returns>
        public static List<GeometryModel3D> cubeInput(GeometryModel3D seed, IArtwork artwork)
        {
            var cubeListInFile = artwork.Cubes;
            List<GeometryModel3D> models = new List<GeometryModel3D>();
            if (cubeListInFile.Count != 0)
                cubeListInFile.ForEach(new Action<ICube>(cb =>
                {
                    GeometryModel3D m = seed.Clone();
                    m.Material = new MaterialGroup();
                    (m.Material as MaterialGroup).Children.Add(
                        new DiffuseMaterial(new SolidColorBrush(cb.CubeColor)));
                    models.Add(m);
                }));
            else
                for(int i = 0; i<artwork.SceneSize.X * artwork.SceneSize.Y * artwork.SceneSize.Z; i++)
                    models.Add(seed.Clone());

            return models;
        }
        #endregion
    }
}
