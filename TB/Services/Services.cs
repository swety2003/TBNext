using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB.Services
{
    public static class ServiceManager
    {
        public static IServiceProvider Value => App.AppHost.Services;

        public static T GetService<T>() where T : class
        {
            if (Value.GetService(typeof(T)) is not T service)
                throw new ArgumentException($"{typeof(T)} Service Not Found");
            return service;
        }

    }
}
