using System;
using System.Runtime.InteropServices;

namespace ScreenAndScreen
{
    public class KeyboardHook
    {
        static int hHook = 0;
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);
        //LowLevel键盘截获，如果是WH_KEYBOARD＝2，并不能对系统键盘截取，Acrobat Reader会在你截取之前获得键盘。 
        HookProc KeyBoardHookProcedure;
        public const int WH_KEYBOARD_LL = 13;
        //键盘Hook结构函数 
        [StructLayout(LayoutKind.Sequential)]
        public class KeyBoardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        //设置钩子 
        [DllImport("user32.dll")]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //抽掉钩子 
        public static extern bool UnhookWindowsHookEx(int idHook);
        [DllImport("user32.dll")]
        //调用下一个钩子 
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        public void HookStart()
        {
            // 安装键盘钩子 
            if (hHook == 0)
            {
                KeyBoardHookProcedure = new HookProc(KeyBoardHookProc);
                hHook = SetWindowsHookEx(WH_KEYBOARD_LL,
                          KeyBoardHookProcedure,
                        GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);
                //如果设置钩子失败. 
                if (hHook == 0)
                {
                    HookClear();
                    //throw new Exception("设置Hook失败!"); 
                }
            }
        }

        //取消钩子事件 
        public void HookClear()
        {
            bool retKeyboard = true;
            if (hHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hHook);
                hHook = 0;
            }
            //如果去掉钩子失败. 
            if (!retKeyboard) throw new Exception("UnhookWindowsHookEx failed.");
        }

        //这里可以添加自己想要的信息处理 
        public static int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {

                // vkCode 是 下面的键位
                // https://docs.microsoft.com/zh-cn/dotnet/api/system.windows.forms.keys?view=netcore-3.1
                KeyBoardHookStruct kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
              
                var flag = 
                    kbh.vkCode == 91 || 
                    kbh.vkCode == 92 || 
                    kbh.vkCode == 115 || 
                    kbh.vkCode == 18 || 
                    kbh.vkCode == 27 || 
                    kbh.vkCode == 17 || 
                    kbh.vkCode == 16 || 
                    kbh.vkCode == 46 || 
                    kbh.vkCode == 241 || 
                    kbh.vkCode == 122 || 
                    kbh.vkCode == 32;
                if (flag) //截获 Ctrl+ Esc + Shift + Delete + F1  + F11 + Space
                {
                    return 1;
                }
              
                //
                //if (kbh.vkCode == (int)Keys.Tab && (int)Control.ModifierKeys == (int)Keys.Alt) //截获alt+tab 
                //{
                //    return 1;
                //}


            }
            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }
    }
}