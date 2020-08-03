
using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ScreenAndScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Window LockWindow { get; set; }
        public Setting _Setting { get; set; }
        public DispatcherTimer _Timer { get; set; } = null;
        public string SettingFullPath { get; set; }  = AppDomain.CurrentDomain.BaseDirectory + "setting.json";
        public double ScreenWidth { get; set; } = SystemParameters.PrimaryScreenWidth;
        public double ScreenHeight { get; set; } = SystemParameters.PrimaryScreenHeight;
        public double WorkAreaWidth { get; set; } = SystemParameters.WorkArea.Width;
        public double WorkAreaHeight { get; set; } = SystemParameters.WorkArea.Height;
        public double WindowWidth { get; set; } = 320;
        public double WindowHeight { get; set; } = 200;
        public KeyboardHook KeyboardHook { get; private set; }

        protected override void OnSourceInitialized(EventArgs e)
        {
            KeyboardHook = new KeyboardHook();
            _Setting = InitSetting();
            StartWhenSystemStart(_Setting.StartUpStatus);
            if (_Setting.StartUpStatus == true)
            {
                StartUpButton.IsEnabled = false;
                CancelStartUpButton.IsEnabled = false;
                StartLockButton.IsEnabled = false;
                ExitButton.IsEnabled = false;
                
                StartUpTip.Opacity = 1;
                this.Width = 10;
                this.Height = 10;
                this.Left = WorkAreaWidth-10;
                this.Top = WorkAreaHeight-10;
                
                this.Opacity = 0.01;
                this.Visibility = Visibility.Hidden;
                HelperUser.HiddenTheAppInToolWindow(this);
                if (_Timer == null)
                {
                    _Timer = new DispatcherTimer();
                    _Timer.Tick += new EventHandler(dispatcherTimer_Tick);
                    _Timer.Interval = new TimeSpan(0, 0, _Setting.StartLockAfterTime);
                    _Timer.Start();
                }
            }
            else
            {
                this.WindowState = WindowState.Normal;
                this.ShowInTaskbar = true;
                StartUpButton.IsEnabled = true;
                StartUpTip.Opacity = 0;
            }
        }

        public MainWindow()
        {

            InitializeComponent();

        }

        public void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            _Timer.Stop();
            Lock();
        }
        private void ExitLock(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }
        
        public Setting InitSetting()
        {

                Setting setting;
                if (File.Exists($@"{SettingFullPath}"))
                {
                    setting = Setting.LoadSetting($@"{SettingFullPath}");
                }
                else
                {
                    setting = new Setting();
                    Setting.SaveSetting($@"{SettingFullPath}", setting);
                }
                return setting;
        }

        private void StartLock(object sender, RoutedEventArgs e)
        {
            Lock();
        }

        private void Open()
        {
            this.Opacity = 1;
            this.Visibility = Visibility.Visible;
            this.Left = (ScreenWidth - WindowWidth) / 2;
            this.Top = (ScreenHeight - WindowHeight) / 2;
            this.Width = WindowWidth;
            this.Height = WindowHeight;
            this.WindowState = WindowState.Normal;
            this.ShowInTaskbar = true;
            HelperUser.ShowTheAppInToolWindow(this);
            KeyboardHook.HookClear();
            CancelStartUpButton.IsEnabled = true;
            StartLockButton.IsEnabled = true;
            ExitButton.IsEnabled = true;

            if (_Setting.StartUpStatus)
            {
                StartUpTip.Opacity = 1;

                StartUpButton.IsEnabled = false;
            } else
            {
                StartUpTip.Opacity = 0;

                StartUpButton.IsEnabled = true;
            }

        }

        public void Lock()
        {
            this.WindowState = WindowState.Minimized;
            LockWindow = new LockWindow(Open);
            LockWindow.AllowsTransparency = true;
            LockWindow.Background = Brushes.White;
            LockWindow.Opacity = 0.01;
            LockWindow.WindowStyle = WindowStyle.None;
            LockWindow.Topmost = true;
            LockWindow.Left = 0;
            LockWindow.Top = 0;
            LockWindow.Width = ScreenWidth;
            LockWindow.Height = ScreenHeight;

            //LockWindow.Background = Brushes.Yellow;
            //LockWindow.Opacity = 0.5;
            //LockWindow.WindowStyle = WindowStyle.None;
            //LockWindow.Topmost = true;
            //LockWindow.Left = 0;
            //LockWindow.Top = 0;
            //LockWindow.Width = 400;
            //LockWindow.Height = 400;


            LockWindow.Show();

            this.ShowInTaskbar = false;
            HelperUser.HiddenTheAppInToolWindow(this);

            KeyboardHook.HookStart();
        }


        private void StartWhenSystemStart(bool flag)
        {
            string programName = "ScreenAndScreen";
            string programPath = Process.GetCurrentProcess().MainModule.FileName;
            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (flag)
            {
                reg.SetValue(programName, programPath);
            }
            else
            {
                reg.DeleteValue(programName, false);
            }
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void StartUpTheApp(object sender, RoutedEventArgs e)
        {
            StartWhenSystemStart(true);
            _Setting.StartUpStatus = true;
            Setting.SaveSetting(SettingFullPath, _Setting);
            StartUpTip.Opacity = 1;
            StartUpButton.IsEnabled = false;
        }
        private void CancelStartUpTheApp(object sender, RoutedEventArgs e)
        {
            StartWhenSystemStart(false);
            _Setting.StartUpStatus = false;
            Setting.SaveSetting(SettingFullPath, _Setting);
            StartUpTip.Opacity = 0;
            StartUpButton.IsEnabled = true;
        }
    }
}
