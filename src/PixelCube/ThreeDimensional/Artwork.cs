using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using PixelCube.Scene3D;
namespace PixelCube.ThreeDimensional
{
    class Artwork:IArtwork
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Artwork()
        {
            init();
        }
        public void init()
        {
        }

        /// <summary>
        /// 背景颜色
        /// </summary>


        public Color BackgroundFill
        {
            get;
            set;
        }




        /// <summary>
        /// 画布的大小
        /// </summary>


        public Vector3D SceneSize
        {
            get;
            set;
        }

  


        /// <summary>
        /// 返回场景中所有的小方块，位于(i, j, k)的小方块的索引为 i+SceneSize.x*j+SceneSize.y*k
        /// </summary>


        public List<ICube> Cubes
        {
            get;
            set;
        }

 

        /// <summary>
        /// 文件目录字符串
        /// </summary>



        public string FileName
        {
            get;
            set;
        }




        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="dt"> Artwork的实体对象</param>
        /// <returns>相对应的字符串</returns>
        public static string Serialize(PixelCube.ThreeDimensional.Artwork dt)
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(dt.GetType());

            StringBuilder sb = new StringBuilder();

            System.IO.StringWriter writer = new System.IO.StringWriter(sb);

            ser.Serialize(writer, dt);

            return sb.ToString();
        }
        /// <summary>
        /// 逆序列化
        /// </summary>
        /// <param name="s">Artwork对应的字符串</param>
        /// <returns>小方块实体对象</returns>
        public static PixelCube.ThreeDimensional.Artwork Deserialize(string s)
        {
            PixelCube.ThreeDimensional.Artwork dt = new PixelCube.ThreeDimensional.Artwork();
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(s);
            System.Xml.XmlNodeReader reader = new System.Xml.XmlNodeReader(xdoc.DocumentElement);
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(dt.GetType());
            object obj = ser.Deserialize(reader);

            return obj as PixelCube.ThreeDimensional.Artwork;
        }
    }
}
