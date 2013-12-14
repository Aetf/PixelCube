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
using PixelCube.Utils;

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
        public ILeapTrace LeapT;

        internal OpCore kernel;
        internal BackgroundMusic bgm;
        internal BackgroundSound se;
        internal WatchDog wd;

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
            kernel.PostEraseOperationEvent += se.EraseOperationSound;

            return se;
        }

        /// <summary>
        /// Must be called after Leap and LeapT is initilized
        /// </summary>
        /// <returns></returns>
        private WatchDog CreateWatchDog()
        {
            var w = new WatchDog();
            w.DoInit(this);
            return w;
        }

        /// <summary>
        /// Must be called after Leap is initilized
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Must be called after Leap is initilized
        /// </summary>
        /// <returns></returns>
        private ILeapTrace CreateLeapTrace()
        {
            var lt = new LeapMenu(Leap);


            lt.Initialize();
            return lt;
        }

        /// <summary>
        /// Must be called after CurrentArt is initilized.
        /// </summary>
        private void InitModules()
        {
            Leap = CreateLeapMotion();
            LeapT = CreateLeapTrace();
            SceneControler = CreateSceneControler();
            kernel = CreateOpCore();
            bgm = CreateBGM();
            se = CreateSE();
            wd = CreateWatchDog();

            SetupSAOMenu();

            Leap.LinkEvent();
            LeapT.LinkEvent();
        }

        /// <summary>
        /// Must be called after LeapT is initialized
        /// </summary>
        private void SetupSAOMenu()
        {
            LeapT.ExhaleMenuEvent += (sender, e) =>
            {
                this.Dispatcher.BeginInvoke(new Action(()=>saomenu.Toggle()));
            };
            LeapT.TraceEvent += (sender, e) =>
            {
                this.Dispatcher.BeginInvoke(new Action(() => { saomenu.Pointer = e.TracePosition.ToPoint3D(); }));
            };
            LeapT.SelectMenuEvent += (sender, e) =>
            {
                this.Dispatcher.BeginInvoke(new Action(()=>saomenu.EnterCurrent()));
            };
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentArt = LSDocu.NewArtwork();

            InitModules();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(Leap != null)
            {
                Leap.Uninitialize();
            }
            if(LeapT != null)
            {
                LeapT.Uninitialize();
            }
        }

        private void MenuItem_Open(object sender, RoutedEventArgs e)
        {
            var tmp = LSDocu.LoadArtworkDoc();
            if(tmp != null)
            {
                CurrentArt = tmp;
                InitModules();
            }
        }

        private void MenuItem_Save(object sender, RoutedEventArgs e)
        {
            LSDocu.SaveAsDocument(CurrentArt);
        }

        private void MenuItem_New(object sender, RoutedEventArgs e)
        {
            CurrentArt = LSDocu.NewArtwork();

            InitModules();
        }

        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            var timer = new System.Timers.Timer(700)
                {
                    AutoReset = false,
                };
            timer.Elapsed += (o, arg) => this.Dispatcher.BeginInvoke(new Action(()=> this.Close()));
            timer.Start();
        }
    }
}
