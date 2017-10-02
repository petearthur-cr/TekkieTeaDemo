using System;
using Cloudreach.Connect.Api;

namespace TekkieTeaDemo
{
    public class PluginLifeCycle : IPluginLifecycle
    {
        public void OnInstallOrUpdate(IPluginContext context)
        {
        }

        public void OnStartup(IPluginContext context)
        {
            PluginConfiguration.Initialise();
            context.LogService.Info("Loaded Configuration");
        }
    }
}
