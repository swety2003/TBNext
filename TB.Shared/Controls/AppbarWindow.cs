using Itp.WpfAppBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using Windows.Win32.Foundation;
using Windows.Win32;

namespace TB.Shared.Controls
{
    public class AppbarWindow : AppBarWindow
    {
        public AppbarWindow()
        {
            Loaded += W_Loaded;
        }


        private HWND desktopHandle;
        private HWND shellHandle;

        public bool RunningFullScreenApp { get; private set; }



        private void W_Loaded(object sender, RoutedEventArgs e)
        {
            desktopHandle = PInvoke.GetDesktopWindow();
            shellHandle = PInvoke.GetShellWindow();
            HwndSource? source = PresentationSource.FromVisual(this) as HwndSource;
            source?.AddHook(WndProc);


        }

        private nint WndProc(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled)
        {
            const int ABN_STATECHANGE = 0x0000000;
            const int ABN_POSCHANGED = 0x0000001;
            const int ABN_FULLSCREENAPP = 0x0000002;
            const int ABN_WINDOWARRANGE = 0x0000003; // lParam == TRUE means hide
            if (msg == AppBarMessageId)
            {
                switch (wParam.ToInt32())
                {
                    case (int)ABN_FULLSCREENAPP:
                        {
                            IntPtr hWnd = PInvoke.GetForegroundWindow();
                            //判断当前全屏的应用是否是桌面
                            if (hWnd.Equals(desktopHandle) || hWnd.Equals(shellHandle))
                            {
                                RunningFullScreenApp = false;
                                break;
                            }
                            //判断是否全屏
                            if ((int)lParam == 1)
                            {
                                this.RunningFullScreenApp = true; this.WindowState = WindowState.Minimized;
                            }
                            else
                            {
                                this.RunningFullScreenApp = false; this.WindowState = WindowState.Normal;
                            }

                            break;
                        }
                    default:
                        break;
                }
            }

            return IntPtr.Zero;
        }

    }
}
