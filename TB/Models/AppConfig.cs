﻿using Newtonsoft.Json;
using System.Collections.Generic;
using TB.Models.Enums;
using TB.Shared.Extra;
using static TB.Services.ServiceManager;
using TB.Services;
using TB.Shared;


namespace TB.Models;

public class AppConfig
{
    public List<WidgetProfile> Status { get; set; }
}


public class WidgetProfile
{
    [JsonProperty("Enabled")] private bool enabledProperty;

    private WidgetPosition posProperty = WidgetPosition.Left;

    public WidgetProfile(string wid, WidgetPosition pos = WidgetPosition.Left)
    {
        Wid = wid;
        Pos = pos;
    }

    [JsonProperty("WID")] public string Wid { get; private set; }

    public WidgetPosition Pos
    {
        get => posProperty;
        set
        {
            if (Enabled)
            {
                GetService<WidgetManager>().Disable(this);

                GetService<WidgetManager>().Enable(this, value);
            }

            posProperty = value;
        }
    }

    [JsonIgnore]
    public IEnumerable<EnumDescription> PosNameList =>
        Enum.GetValues(typeof(WidgetPosition))
            .Cast<WidgetPosition>()
            .Select(e => new EnumDescription(e));

    [JsonIgnore]
    public bool Enabled
    {
        get => enabledProperty;
        set
        {
            enabledProperty = value;
            if (value)
                GetService<WidgetManager>().Enable(this, Pos);
            else
                GetService<WidgetManager>().Disable(this);
        }
    }

    [JsonIgnore]
    public WidgetMainfest Mainfest => GetService<PluginLoader>().WidgetMainfests
        .Where(x => x.Widget.FullName == Wid).Select(x => x).First();
}