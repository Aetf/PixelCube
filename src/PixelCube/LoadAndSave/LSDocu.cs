using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using PixelCube.ThreeDimensional;


namespace PixelCube.LoadAndSave
{
    class LSDocu
    {
         public LSDocu()
        {
            
        }

         Artwork ArtworkDoc;
        //openFileDialog
         private void LoadArtworkDoc()
        {
            ArtworkDoc = new Artwork();
            try
            {
                //打开文件对话框选取pd文件
                OpenFileDialog openFileDialog2;
                openFileDialog2 = new OpenFileDialog();
                openFileDialog2.Title = "情选择要打开的文件";
                openFileDialog2.Filter = "PixcelCube Documents(*.pd)|*.pd";
                openFileDialog2.ShowDialog();
                //将数据载入Document
                if (openFileDialog2.ShowDialog() == DialogResult.OK)
                {

                    string fileName = openFileDialog2.FileName;
                    Stream stream = File.OpenRead(fileName);
                    if (!File.Exists(fileName))
                    {
                        MessageBox.Show("文件不存在");
                        return;
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
          
        }

        //save 
         private void SaveDocument(Artwork ArtworkDoc)
        {
            //check that the document is not read only
            string str = Serialize(ArtworkDoc);
                 try
                    {
                        String fileName = ArtworkDoc.FileName;                    
                        Stream stream = File.OpenWrite(fileName);
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
        //saveas
         private void SaveAsDocument(Artwork ArtworkDoc)
        {
            //open a file dialog for saving map document
            SaveFileDialog SaveFileDialog1= new SaveFileDialog();
            SaveFileDialog1.Title = "将文件另存于";
            SaveFileDialog1.Filter = "Map Documents(*.pd)|*.pd";
            SaveFileDialog1.ShowDialog();
            string str=Serialize(ArtworkDoc);
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
