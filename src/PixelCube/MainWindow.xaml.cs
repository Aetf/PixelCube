using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using PixelCube.LeapMotion;
using PixelCube.Scene3D;
using HelixToolkit.Wpf;
using PixelCube.LoadAndSave;
using PixelCube.Operations;
using PixelCube.Sound;
using System;

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

        internal OpCore kernel;
        internal BackgroundMusic bgm;
        internal BackgroundSound se;

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
            var isc = new CubeSceneController();
            isc.DoInit(this);
            return isc;
        }

        private ILeapMotion CreateLeapMotion()
        {
            var leap = new LeapController(CurrentArt);
            leap.Initialize();
            return leap;
        }

        private BackgroundMusic CreateBGM()
        {
            var bgm = new BackgroundMusic();
            bgm.DoInit(this, true);

            return bgm;
        }

        /// <summary>
        /// Must be called after kernel is initialized.
        /// </summary>
        /// <returns></returns>
        private BackgroundSound CreateSE()
        {
            var se = new BackgroundSound();
            se.DoInit(this);

            kernel.PostDrawOperationEvent += se.DrawOperationSound;
            kernel.PostFocusOperationEvent += se.FocusOperationSound;

            // FIXME: OpCore donot have PostEraseOperationEvent.
            //kernel.PostEraseOperationEvent += se.EraseOperationSound;

            return se;
        }

        private OpCore CreateOpCore()
        {
            var c = new OpCore();
            c.DoInit(this);

            // Link all the event listeners.
            Leap.PreChangeColorOperationEvent += c.OnChangeColorOperation;
            Leap.PreDragOperationEvent += c.OnDragOperation;
            Leap.PreDrawOperationEvent += c.OnPreDrawOperation;
            Leap.PreEraseOperationEvent += c.OnEraseOperation;
            Leap.PreRotateOperationEvent += c.OnPreRotateOperation;
            Leap.PreScaleOperationEvent += c.OnPreScaleOperation;
            Leap.PreFocusOperationEvent += c.OnPreFocusOperation;
           
            return c;
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentArt = LSDocu.NewArtwork();
            Leap = CreateLeapMotion();
            SceneControler = CreateSceneControler();
            kernel = CreateOpCore();
            bgm = CreateBGM();
            se = CreateSE();

            Leap.PreFocusOperationEvent += new System.EventHandler<PreFocusOperationEventArgs>((obj, arg)=>
            {
                this.Dispatcher.BeginInvoke(new Action(()=>
                {
                    pointer.Center = new Point3D(arg.FocusPosition.x, arg.FocusPosition.y, arg.FocusPosition.z);
                }));
            });

            Leap.LinkEvent();
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
