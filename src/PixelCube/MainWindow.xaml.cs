using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using PixelCube.LeapMotion;
using PixelCube.LoadAndSave;
using PixelCube.Operations;
using PixelCube.Scene3D;
using PixelCube.Sound;

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
            leap.LeapConnectionChangedEvent += (o, e) =>
            {
                this.Dispatcher.BeginInvoke(new Action(() => this.WaitLeap(e.Connected)));
            };

            // Initilize in another thread not to block ui thread.
            ThreadPool.QueueUserWorkItem(new WaitCallback(o => leap.Initialize()));
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
        /// Must be called after SceneController is initilized
        /// </summary>
        /// <returns></returns>
        private OpCore CreateOpCore()
        {
            var c = new OpCore();
            c.DoInit(this);

            return c;
        }

        /// <summary>
        /// Must be called after Leap is initilized
        /// </summary>
        /// <returns></returns>
        private ILeapTrace CreateLeapTrace()
        {
            var lt = new LeapMenu(Leap);
            lt.DoInit();
            return lt;
        }

        /// <summary>
        /// Must be called after LeapT is initialized
        /// </summary>
        private void SetupSAOMenu()
        {
            LeapT.ExhaleMenuEvent += (sender, e) =>
            {
                this.Dispatcher.BeginInvoke(new Action(()=>saomenu.Show()));
            };
            LeapT.TraceEvent += (sender, e) =>
            {
                Point3D tipPos = e.TracePosition;
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    saomenu.RawPointer = SceneControler.WorldTransform.Transform(tipPos);
                }));
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    slotmenu.RawPointer = SceneControler.WorldTransform.Transform(tipPos);
                }));
            };
            LeapT.SelectMenuEvent += (sender, e) =>
            {
                this.Dispatcher.BeginInvoke(new Action(()=>saomenu.EnterCurrent()));
                this.Dispatcher.BeginInvoke(new Action(()=>slotmenu.EnterCurrent()));
            };
        }

        /// <summary>
        /// Must be called after CurrentArt is initilized.
        /// </summary>
        private void InitModules()
        {
            Leap = CreateLeapMotion();
            LeapT = CreateLeapTrace();
            wd = CreateWatchDog();
            SetupSAOMenu();

            SceneControler = CreateSceneControler();
            kernel = CreateOpCore();
            se = CreateSE();
        }
        #endregion

        private void ResetWorld()
        {
            cubeGroup.Children.Clear();
            if (Leap != null)
            {
                Leap.Uninitialize();
            }
            if (LeapT != null)
            {
                LeapT.Uninitialize();
            }
            mCamera.Position = new Point3D(20, 20, 110);
            mCamera.LookDirection = new Vector3D(0, 0, -1);
            mCamera.UpDirection = new Vector3D(0, 1, 0);
        }

        private void WaitLeap(bool connected)
        {
            if(connected)
            {
                waitingimg.Visibility = Visibility.Collapsed;
            }
            else
            {
                waitingimg.Visibility = Visibility.Visible;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bgm = CreateBGM();
            waitingimg.Focus();
            Keyboard.AddKeyDownHandler(this, waiting_KeyDown);

            CurrentArt = LSDocu.NewArtwork();
            // Post init event in message queue
            // not to block the window.
            this.Dispatcher.BeginInvoke(new Action(InitModules));
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
            var tmp = LSDocu.LoadArtworkDoc(ConfigProvider.Instance.SlotPath[0]);
            if(tmp != null)
            {
                ResetWorld();
                CurrentArt = tmp;
                InitModules();
            }
        }

        private void MenuItem_Save(object sender, RoutedEventArgs e)
        {
            CurrentArt.FileName = ConfigProvider.Instance.SlotPath[0];
            LSDocu.SaveDocument(CurrentArt);
        }

        private void MenuItem_New(object sender, RoutedEventArgs e)
        {
            CurrentArt = LSDocu.NewArtwork();
            ResetWorld();
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

        private void waiting_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (waitingimg.Visibility != Visibility.Visible)
                return;

            switch(e.Key)
            {
            case System.Windows.Input.Key.Escape:
                    CurrentArt.FileName = ConfigProvider.Instance.SlotPath[1];
                    LSDocu.SaveDocument(CurrentArt);
                    this.Close();
                break;
            }
        }
    }
}
