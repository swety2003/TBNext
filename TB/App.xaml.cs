using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;
using TB.Services;
using TB.Shared;
using TB.ViewModels;
using static TB.Services.ServiceManager;

namespace TB
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static ILogger<App> _logger;

        static App()
        {
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
#endif
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders()
                        .AddSimpleConsole(options =>
                        {
                            options.SingleLine = true;
                            options.IncludeScopes = true;
                            options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                            options.UseUtcTimestamp = false;
                        }).AddDebug();

                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .UseContentRoot(AppContext.BaseDirectory).ConfigureServices
                ((context, services) =>
                {
                    //services.AddSingleton<AppConfigService>();
                    //services.AddSingleton<WidgetContainerService>();
                    //services.AddSingleton<PluginLoader>();
                    Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();

                    // 过滤出特定命名空间下的类型（类）
                    var svs = allTypes.Where(type => type.Namespace == "TB.Services" && !(type.IsAbstract && type.IsSealed && type.IsClass));
                    foreach ( var type in svs)
                    {
                        services.AddSingleton(type);
                    }

                    #region ViewModel

                    var vms = allTypes.Where(type => type.Namespace == "TB.ViewModels");
                    foreach (var type in vms)
                    {
                        services.AddSingleton(type);
                    }
                    //services.AddSingleton<SettingsWindowViewModel>();
                    //services.AddSingleton<ItemManageVM>();
                    //services.AddSingleton<CardManageVM>();
                    //services.AddSingleton<InstalledCardsVM>();
                    //services.AddSingleton<PreferenceVM>();
                    //services.AddSingleton<SideBarManageVM>();

                    #endregion

                    //services.AddTransient<SettingWindow>();
                    //services.AddTransient<CardManage>();
                    //services.AddTransient<PreferencePage>();

                    //services.AddTransient<Settings>();
                    //services.AddTransient<AboutPage>();
                    //services.AddTransient<InstalledCards>();
                    //services.AddTransient<SideBarManage>();
                }).Build();


            _logger = GetService<ILoggerFactory>().CreateLogger<App>();

            bool createNew;
            var _ = new Mutex(true, "swety.yasb.app", out createNew);
            if (!createNew)
            {
                MessageBox.Show("Application is already run!");
                Environment.Exit(0);

            }


            GetService<AppConfigManager>().Load();
            GetService<PluginLoader>().Load();
        }

        private static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            throw e.Exception;

        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw e.ExceptionObject as Exception;

        }

        internal static IHost AppHost { get; }

        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            GetService<AppConfigManager>().Config.Status =
                GetService<SettingsWindowViewModel>().TopBarStatuses.ToList();
            GetService<AppConfigManager>().Save();
            //foreach (var item in GetService<AppConfigManager>().Config.Status) item.Enabled = false;
        }
    }

}
