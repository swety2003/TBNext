using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TB.Shared;
using TB.Shared.Controls;
using WindowsDesktop;
using WpfLibrary1.ViewModels;

namespace WpfLibrary1.Views
{
    /// <summary>
    /// VirtualDesktopHelper.xaml 的交互逻辑
    /// </summary>
    [Widget("虚拟桌面助手","23333")]
    public partial class VirtualDesktopHelper : MyViewBase<VirtualDesktopHelperViewModel>
    {
        static VirtualDesktopHelper()
        {

            AppDomain.CurrentDomain.AssemblyResolve += (_, args) => args.Name.StartsWith("VirtualDesktop") ?
                Assembly.GetAssembly(typeof(VirtualDesktop)) : null;
        }
        public VirtualDesktopHelper()
        {
            InitializeComponent();

        }
    }
}
