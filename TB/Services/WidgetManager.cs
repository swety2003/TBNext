using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TB.Models;
using TB.Models.Enums;
using TB.Shared;
using static TB.Services.ServiceManager;

namespace TB.Services;

internal class WidgetManager
{
    internal StackPanel? LeftPanel { get; set; }

    internal StackPanel? CenterPanel { get; set; }

    internal StackPanel? RightPanel { get; set; }

    private Dictionary<WidgetPosition, StackPanel> PanelMap { get; } = new();

    private IList<WidgetProfile> topBarStatusList { get; } = new List<WidgetProfile>();

    internal IList<WidgetProfile> WidgetStatusList => topBarStatusList;

    private IList<UIElement> activeItems { get; } = new List<UIElement>();

    public void InitTopBarContainerService()
    {
        PanelMap[WidgetPosition.Left] = LeftPanel ?? throw new Exception();
        PanelMap[WidgetPosition.Center] = CenterPanel ?? throw new Exception();
        PanelMap[WidgetPosition.Right] = RightPanel ?? throw new Exception();

        foreach (var item in GetService<PluginLoader>().WidgetMainfests)
            topBarStatusList.Add(new WidgetProfile(item.Widget.FullName ?? throw new Exception()));
    }


    internal event PropertyChangedEventHandler? PropertyChanged;

    internal void Enable(WidgetProfile ts, WidgetPosition pos)
    {
        var card = Activator.CreateInstance(ts.Mainfest.Widget) as UIElement;
        PanelMap[pos].Children.Add(card as UserControl ?? throw new Exception());
        activeItems.Add(card);
        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(WidgetStatusList)));
    }


    public void Disable(WidgetProfile tbs)
    {
        try
        {
            var c = activeItems.Where(x => x.GetType() == tbs.Mainfest.Widget).First();
            Disable(c, tbs.Pos);
        }
        catch (Exception ex)
        {
            //logger.LogError(ex, "禁用失败");
        }
    }

    public void Disable(UIElement c, WidgetPosition pos)
    {
        PanelMap[pos].Children.Remove(c);

        activeItems.Remove(c);

        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(WidgetStatusList)));
    }
}