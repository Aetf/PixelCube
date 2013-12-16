using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PixelCube.Scene3D;
using PixelCube.ThreeDimensional;

namespace PixelCube.LoadAndSave
{
    static class LSDocu
    {
        /// <summary>
        /// 对工程文件的新建
        /// </summary>
        static public IArtwork NewArtwork()
        {
            Artwork ArtworkDoc = new Artwork();
            ArtworkDoc.DefaultValue();

            return ArtworkDoc;
        }

        /// <summary>
        /// 从指定路径加载
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public IArtwork LoadArtworkDoc(String path)
        {
            IArtwork ArtworkDoc = null;
            if (!File.Exists(path))
            {
                MessageBox.Show("文件不存在");
            }
            else
            {
                try
                {
                    Stream stream = File.OpenRead(path);
                    StreamReader reader = new StreamReader(stream);
                    string str = reader.ReadToEnd();
                    ArtworkDoc = Deserialize(str);
                    ArtworkDoc.FileName = path;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return ArtworkDoc;
        }

        /// <summary>
        /// 文件的加载, 可能返回null
        /// </summary>
        static public IArtwork LoadArtworkDoc()
        {
            IArtwork ArtworkDoc = null;
            try
            {
                //打开文件对话框选取pd文件
                OpenFileDialog openFileDialog2;
                openFileDialog2 = new OpenFileDialog();
                openFileDialog2.Title = "请选择要打开的文件";
                openFileDialog2.Filter = "PixcelCube Documents(*.pd)|*.pd";
                //将数据载入Document
                if (openFileDialog2.ShowDialog() == DialogResult.OK)
                {

                    string fileName = openFileDialog2.FileName;
                    Stream stream = File.OpenRead(fileName);
                    if (!File.Exists(fileName))
                    {
                        MessageBox.Show("文件不存在");
                    }
                    else
                    {
                        StreamReader reader = new StreamReader(stream);
                        string str = reader.ReadToEnd();
                        ArtworkDoc = Deserialize(str);
                        ArtworkDoc.FileName = fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return ArtworkDoc;
        }

        /// <summary>
        /// 文件保存
        /// </summary>
        /// <param name="ArtworkDoc"></param>
        static public void SaveDocument(IArtwork ArtworkDoc)
        {
            //check that the document is not read only
            string str = Serialize(ArtworkDoc);
            try
            {
                String fileName = ArtworkDoc.FileName;
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                Stream stream = File.Open(fileName, FileMode.Create);
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.BaseStream.Flush();
                    writer.Write(str);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Simple Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 文件另存为
        /// </summary>
        /// <param name="ArtworkDoc"></param>
        static public void SaveAsDocument(IArtwork ArtworkDoc)
        {
            //open a file dialog for saving document
            SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
            SaveFileDialog1.Title = "将文件另存于";
            SaveFileDialog1.Filter = "Map Documents(*.pd)|*.pd";
            if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = SaveFileDialog1.FileName;
                if (fileName == "")
                {
                    MessageBox.Show("请填写文件名");
                }
                else
                {
                    try
                    {
                        string str = Serialize(ArtworkDoc);
                        Stream stream = File.OpenWrite(fileName);
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(str);
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message, "Simple Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="dt"> Artwork的实体对象</param>
        /// <returns>相对应的字符串</returns>
        public static string Serialize(IArtwork dt)
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
