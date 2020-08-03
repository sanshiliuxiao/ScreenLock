using System;
using System.Windows;

namespace ScreenAndScreen
{
    /// <summary>
    /// Lock.xaml 的交互逻辑
    /// </summary>
    public partial class LockWindow : Window
    {
        public Window _verifyPasswordWindow { get; set; }
        public Action _openMainWindow { get; set; }
        public LockWindow()
        {
            InitializeComponent();

        }
        public LockWindow(Action openMainWindow)
        {
            _openMainWindow = openMainWindow;
        
            InitializeComponent();
        }

        private void CloseLock(object sender, RoutedEventArgs e)
        {
            _verifyPasswordWindow = new VerifyPassword(CLoseCurrentWindow);
            _verifyPasswordWindow.Topmost = true;
            _verifyPasswordWindow.ShowDialog();
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            HelperUser.HiddenTheAppInToolWindow(this);
        }
        private void CLoseCurrentWindow()
        {

            _openMainWindow();
            this.Close();

        }

        private void CloseVerifyWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
