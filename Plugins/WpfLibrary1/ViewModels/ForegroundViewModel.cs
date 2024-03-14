using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;
using System.Windows.Interop;
using TB.Shared.Utils;
using static Windows.Win32.PInvoke;

using Windows.Win32.Foundation;
using Windows.Win32.UI.Accessibility;
using WpfLibrary1.Extra;
using System.Windows.Media;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;


namespace WpfLibrary1.ViewModels
{
    public partial class ForegroundViewModel : ViewModelBase
    {
        public override void Init()
        {
            base.Init();
            OnEnabled();
        }
        [ObservableProperty] private WindowItem data;

        [ObservableProperty] ImageSource icon;
        public override void Update()
        {
        }


        private WINEVENTPROC _hookProc;
        internal void OnEnabled()
        {
            _hookProc = WinEventProc;
            // 监听系统的前台窗口变化。
            SetWinEventHook(
                EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND,
                HMODULE.Null, _hookProc,
                0, 0,
                WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);

            // 开启消息循环，以便 WinEventProc 能够被调用。
            if (GetMessage(out var lpMsg, default, default, default))
            {
                TranslateMessage(in lpMsg);
                DispatchMessage(in lpMsg);
            }
        }

        // 当前前台窗口变化时，输出新的前台窗口信息。
        void WinEventProc(HWINEVENTHOOK hWinEventHook, uint @event, HWND hwnd, int idObject, int idChild, uint idEventThread, uint dwmsEventTime)
        {
            var current = GetForegroundWindow();

            var w = new Win32Window(current);
            // 你也可以获得更多你想获得的信息，这里我只是举例输出了几个而已。
            // var rowText = $"[{w.Handle}] {w.Title} - {w.ProcessName}";
            Data = new WindowItem(w.Handle, w.Title);
            GetWindowThreadProcessId(hwnd, out int processId);
            Process process = Process.GetProcessById(processId);

            // 检查进程是否存在并获取其主模块（通常是可执行文件）的路径
            if (process != null && !process.HasExited)
            {
                var file= process.MainModule.FileName;
                if (File.Exists(file))
                {

                    Icon = FileIconUtil.GetImgByFile(file);
                }
            }
        }
        public record WindowItem(IntPtr handle, string title);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
    }
}