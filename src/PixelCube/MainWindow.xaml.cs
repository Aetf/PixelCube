using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PixelCube.Scene3D;
using PixelCube.LeapMotion;
using System.Windows.Media.Media3D;

namespace PixelCube
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public ISceneControler SceneControler;
        public IArtwork CurrentArt;
        public ILeapMotion Leap;

        public Viewport3D getViewport()
        {
            return sceneViewport;
        }

        public Model3DGroup getCubeGroup()
        {
            return cubeGroup;
        }


        public MainWindow()
        {
            InitializeComponent();
        }

        #region Modules Initializatoin
        private ISceneControler CreateSceneControler()
        {
            ISceneControler isc = new CubeSceneControler();
            isc.DoInit(this);
            return isc;
        }

        private ILeapMotion CreateLeapMotion()
        {
            return null;
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Leap = CreateLeapMotion();
            SceneControler = CreateSceneControler();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Leap.Uninitialize();
        }
    }
}
