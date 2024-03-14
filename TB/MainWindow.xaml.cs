using TB.ViewModels;
using Itp.WpfAppBar;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TB.Models;
using TB.Services;
using TB.Shared.Controls;
using TB.Views;

namespace TB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AppbarWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            this.cbEdge.ItemsSource = new[]
            {
                AppBarDockMode.Left,
                AppBarDockMode.Right,
                AppBarDockMode.Top,
                AppBarDockMode.Bottom
            };
            this.cbMonitor.ItemsSource = MonitorInfo.GetAllMonitors();
            this.cbMonitor.SelectedIndex = 0;

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            var tbcs = ServiceManager.GetService<WidgetManager>();
            tbcs.RightPanel = right_area;
            tbcs.LeftPanel = left_area;
            tbcs.CenterPanel = center_area;
            tbcs.InitTopBarContainerService();

            var tbs = ServiceManager.GetService<SettingsWindowViewModel>().TopBarStatuses;

            foreach (var item in ServiceManager.GetService<AppConfigManager>().Config.Status ?? new List<WidgetProfile>())
                foreach (var i1 in tbs)
                    if (item.Enabled && i1.Wid == item.Wid)
                    {
                        i1.Pos = item.Pos;
                        i1.Enabled = true;
                    }
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.WindowState = WindowState.Minimized;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().Show();
        }
    }

}