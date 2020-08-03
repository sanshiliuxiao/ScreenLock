using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;

namespace ScreenAndScreen
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public class MessageAutoHidden
    {
        const int WM_CLOSE = 0x0010;
        Timer _timeoutTimer;
        string _caption;
        public void Show(string text, string caption = "提示", int timeout = 1000)
        {
            _caption = caption;
            _timeoutTimer = new Timer(OnTimerElapsed,
            null, timeout, Timeout.Infinite);
            MessageBox.Show(text, caption);
        }

        private void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow(null, _caption);
            if (mbWnd != IntPtr.Zero)
            {
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
        }

        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }
}

