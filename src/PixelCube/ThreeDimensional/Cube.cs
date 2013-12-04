﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using PixelCube.Scene3D;

namespace PixelCube.ThreeDimensional
{
    class Cube:ICube
    {
        
        /// <summary>
        /// 构造小方块及初始化
        /// </summary>
        public Cube()
        {
            Init();
        }
        public void Init()
        {
            visible = true;
            hasfocus = true;
        }

        /// <summary>
        /// 小方块相对场景的位置。
        /// 比如在4x4x4的场景中，某个角即为(0, 0, 0)
        /// </summary>
        private Vector3D Position;

        public Vector3D Position1
        {
            get { return Position; }
            set { Position = value; }
        }
       

        /// <summary>
        /// 小方块的颜色
        /// </summary>
        private Color CubeColor;

        public Color CubeColor1
        {
            get { return CubeColor; }
            set { CubeColor = value; }
        }


        /// <summary>
        /// 是否可见
        /// </summary>
        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        /// <summary>
        /// 是否拥有焦点
        /// </summary>
        private bool hasfocus;
        public bool Hasfocus
        {
            get { return hasfocus; }
            set { hasfocus = value; }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="dt"> 小方块的实体对象</param>
        /// <returns>相对应的字符串</returns>
        public static string Serialize(PixelCube.ThreeDimensional.Cube dt)
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
        /// <param name="s">小方块对应的字符串</param>
        /// <returns>小方块实体对象</returns>
        public static PixelCube.ThreeDimensional.Cube Deserialize(string s)
        {
            PixelCube.ThreeDimensional.Cube dt = new PixelCube.ThreeDimensional.Cube();
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(s);
            System.Xml.XmlNodeReader reader = new System.Xml.XmlNodeReader(xdoc.DocumentElement);
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(dt.GetType());
            object obj = ser.Deserialize(reader);

            return obj as PixelCube.ThreeDimensional.Cube;
        }

    }
}
