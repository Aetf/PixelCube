using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Media.Media3D;

namespace PixelCube
{
    /// <summary>
    /// 提供和读取应用的所有设置信息
    /// 使用ConfigProvider.Instance来获取一个实例
    /// </summary>
    class ConfigProvider
    {
        private static Configuration config;
        private static AppSettingsSection appSettings;

        static ConfigProvider()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            appSettings = config.AppSettings as AppSettingsSection;

            slotpath = new List<String>();
            for(int i = 0; i!= 2; i++)
            {
                var slot = Path.Combine(new String[]{
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "PixelCube", "slot" + i});
                slotpath.Add(slot);
            }
        }

        private ConfigProvider() { }

        public static readonly ConfigProvider Instance = new ConfigProvider();

        #region Helper Methods
        private void write<T>(string property, T value)
        {
            lock (this)
            {
                appSettings.Settings[property].Value = value.ToString();
                config.Save();
            }
        }

        private int readInt(string property, int defaultValue)
        {
            int t;
            if (!int.TryParse(appSettings.Settings[property].Value, out t))
                t = defaultValue;
            return t;
        }

        private bool readBool(string property, bool defaultValue)
        {
            bool t;
            if (!bool.TryParse(appSettings.Settings[property].Value, out t))
                t = defaultValue;
            return t;
        }

        private double readDouble(string property, double defaultValue)
        {
            double t;
            if (!double.TryParse(appSettings.Settings[property].Value, out t))
                t = defaultValue;
            return t;
        }

        private string readString(string property, string defaultValue)
        {
            return appSettings.Settings[property].Value;
        }
        #endregion

        #region public double BGMVolume
        /// <summary>
        /// BGM 音量，范围0-100
        /// </summary>
        public double BGMVolume
        {
            get
            {
                return readDouble("BGMVolume", 50);
            }
        }
        #endregion

        #region public double BGMMute
        /// <summary>
        /// BGM 是否静音
        /// </summary>
        public bool BGMMute
        {
            get
            {
                return readBool("BGMMute", false);
            }
        }
        #endregion

        #region public double SEVolume
        /// <summary>
        /// 音效音量，范围0-100
        /// </summary>
        public double SEVolume
        {
            get
            {
                return readDouble("SEVolume", 50);
            }
        }
        #endregion

        #region public double SEMute
        /// <summary>
        /// 操作音效是否静音
        /// </summary>
        public bool SEMute
        {
            get
            {
                return readBool("SEMute", false);
            }
        }
        #endregion

        #region public Rect3D LeapWorldRect
        /// <summary>
        /// LeapMotion坐标系的坐标范围
        /// </summary>
        public Rect3D LeapWorldRect
        {
            get
            {
                return new Rect3D(-300, 30, -300, 600, 600, 600);
            }
        }
        #endregion

        #region public List<String> SlotPath
        private static List<String> slotpath;
        /// <summary>
        /// 返回可用的Slot的路径列表
        /// </summary>
        public List<String> SlotPath
        {
            get
            {
                return slotpath;
            }
        }
        #endregion

        #region public double CubeA
        /// <summary>
        /// 小方块边长
        /// </summary>
        public double CubeA
        {
            get
            {
                return (Double) App.Current.FindResource("cubeA");
            }
        }
        #endregion
    }
}
