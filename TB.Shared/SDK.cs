
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

namespace TB.Shared
{

    //public record WidgetMainfest(string Name, string Description, Type Widget);

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WidgetAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public WidgetAttribute(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }


    public record WidgetMainfest(string Name, string Description, Type Widget,PluginInfo PluginInfo);

    public class PluginInfo
    {
        private readonly Assembly assembly;

        public string? GetLocation()
        {
            try
            {

                return new DirectoryInfo(Path.GetDirectoryName(assembly.Location)).Parent?.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public PluginInfo(Assembly assembly)
        {

            name = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? "";
            author = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? "";
            desc = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? "";
            version = assembly.GetName().Version ?? new Version();
            this.assembly = assembly;
        }
        public string name { get; init; }
        public string desc { get; init; }
        public Version version { get; init; }
        public string url { get; init; }
        public string author { get; init; }

        public IEnumerable<WidgetMainfest> GetAllTypeInfo()
        {
            List<WidgetMainfest> ret = new List<WidgetMainfest>();
            List<(Type widget, WidgetAttribute? WidgetInfo)> widgetTypes = assembly.GetTypes()
            .Where(type => type.IsDefined(typeof(WidgetAttribute), false))
            .Select(type =>
            {
                var attribute = type.GetCustomAttribute<WidgetAttribute>();
                return (type, attribute);
            })
            .ToList();
            foreach (var attr in widgetTypes)
            {
                if (attr.WidgetInfo == null) continue;
                ret.Add(new WidgetMainfest(attr.WidgetInfo.Name,attr.WidgetInfo.Description,attr.widget,this));
            }

            return ret;
        }
    }

    /// <summary>
    /// </summary>
    public static class Logger
    {
        private static ILoggerFactory? _loggerFactory;

        public static ILoggerFactory? LoggerFactory
        {
            get => _loggerFactory;
            set
            {
                if (_loggerFactory == null) _loggerFactory = value;
            }
        }

        public static ILogger<T> CreateLogger<T>()
        {
            return LoggerFactory?.CreateLogger<T>() ?? throw new Exception("Logger.LoggerFactory Œ¥≥ı ºªØ£°");
        }
    }

}
