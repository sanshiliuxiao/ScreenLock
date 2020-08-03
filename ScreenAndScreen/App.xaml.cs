
using System;
using System.Threading;
using System.Windows;

namespace ScreenAndScreen
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex mutex;

        public App()
        {
            this.Startup += new StartupEventHandler(AppStartup);
        }

        private void AppStartup(object sender, StartupEventArgs e)
        {
            bool ret;
            mutex = new Mutex(true, "ElectronicNeedleTherapySystem", out ret);

            if (!ret)
            {
                var msg = new MessageAutoHidden();
                msg.Show("已有一个程序实例运行");
                Environment.Exit(0);

            }
        }
    }
}
