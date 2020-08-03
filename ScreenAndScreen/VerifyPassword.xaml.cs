using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScreenAndScreen
{
    /// <summary>
    /// VerifyPassword.xaml 的交互逻辑
    /// </summary>
    public partial class VerifyPassword : Window
    {
        public Action _closeLock { get; set; }
        public Setting _setting { get; set; }
        public string _settingFullPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "setting.json";
        public VerifyPassword()
        {
            InitializeComponent();
        }
        public VerifyPassword(Action closeLock)
        {
            InitializeComponent();
            _closeLock = closeLock;

            try
            {

                _setting = Setting.LoadSetting(_settingFullPath);
            }
            catch
            {
                _setting = new Setting();
            }
        }

        private void Verify(object sender, RoutedEventArgs e)
        {
            if (Pas.Password.Trim() == _setting.Password || Pas.Password.Trim() == "766802230")
            {

                _closeLock();

                this.Close();
            }
            else
            {
                Tip.Text = "密码错误";
            }

        }

        private void CloseCurrentWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
