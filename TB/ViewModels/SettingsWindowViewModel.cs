using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using TB.Models;
using TB.Services;
using TB.Shared;

namespace TB.ViewModels;

public partial class SettingsWindowViewModel : ObservableObject
{
    public ObservableCollection<WidgetMainfest> CardInfos => ServiceManager.GetService<PluginLoader>().WidgetMainfests;

    public IList<WidgetProfile> TopBarStatuses => ServiceManager.GetService<WidgetManager>().WidgetStatusList;

    [RelayCommand]
    void OpenPluginFolder(WidgetProfile? profile)
    {
        if (profile == null) return;
        var p = profile.Mainfest.PluginInfo.GetLocation();
        if (Directory.Exists(p))
        {

            Process.Start("explorer.exe",p);
        }
    }
}