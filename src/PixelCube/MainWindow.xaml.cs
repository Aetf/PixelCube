using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using PixelCube.LeapMotion;
using PixelCube.Scene3D;
using HelixToolkit.Wpf;
using PixelCube.LoadAndSave;

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

        public HelixViewport3D getViewport()
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
            // FIXME: need to change LeapController constructor to use
            // IArtwork interface instead of Artwork class.
            ILeapMotion leap = null;
            //leap = new LeapController(CurrentArt);
            //leap.Initialize();
            return leap;
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentArt = LSDocu.NewArtwork();
            Leap = CreateLeapMotion();
            SceneControler = CreateSceneControler();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(Leap != null)
            {
                Leap.Uninitialize();
            }
        }
    }
}
