using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Foreman.PluginManager;
using Foreman.Server.Data;
using Foreman.Shared.Data.Plugin;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foreman.Server.Utility
{
    public static class ServicePluginExtension
    {
        public static IServiceCollection LoadPlugins(this IServiceCollection services, IConfiguration configuration)
        {
            List<Plugin> foundPlugins = new List<Plugin>();
            using (ApplicationContext db = services.BuildServiceProvider()
                    .GetService<ApplicationContext>())
            {
                foreach (string p in GetPluginDirectories())
                {
                    Assembly assembly = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", p, p + ".dll"));
                    var part = new AssemblyPart(assembly);
                    services.AddControllers().PartManager.ApplicationParts.Add(part);
                    PluginActionDescriptorChangeProvider.Instance.HasChanged = true;
                    PluginActionDescriptorChangeProvider.Instance?.TokenSource?.Cancel();
                    var atypes = assembly.GetTypes();
                    var pluginClass = atypes.SingleOrDefault(t => t.GetInterface(nameof(IPlugin)) != null);

                    if (pluginClass != null)
                    {
                        var initMethod = pluginClass.GetMethod(nameof(IPlugin.Initialize), BindingFlags.Public | BindingFlags.Instance);
                        var migrationMethod = pluginClass.GetMethod(nameof(IPlugin.Migrate), BindingFlags.Public | BindingFlags.Instance);
                        var obj = Activator.CreateInstance(pluginClass);
                        var pluginName = (string)pluginClass.GetMethod(nameof(IPlugin.GetPluginName)).Invoke(obj, null);
                        var pluginVersion = (string)pluginClass.GetMethod(nameof(IPlugin.GetPluginVersion)).Invoke(obj, null);
                        var pluginIcon = (string)pluginClass.GetMethod(nameof(IPlugin.GetPluginIcon)).Invoke(obj, null);
                        var pluginDescription = (string)pluginClass.GetMethod(nameof(IPlugin.GetPluginDescription)).Invoke(obj, null);

                        //context.Add(new Shared.Data.Plugin.Plugin() { Name = pluginName, Version = pluginVersion });
                        //context.SaveChanges();
                        initMethod.Invoke(obj, new object[] { services, configuration });
                        
                        if(db.Database.CanConnect())
                        {
                            var dbp = db.Plugins.FirstOrDefault(x => x.Name == pluginName);

                            if (dbp == null)
                            {
                                db.Add(new Plugin() { Name = pluginName, Version = pluginVersion, Icon = pluginIcon, Description = pluginDescription });
                                migrationMethod.Invoke(obj, new object[] { services });
                            }
                            else if (dbp.Version != pluginVersion)
                            {
                                dbp.Version = pluginVersion;
                                db.Update(dbp);
                            }
                        }
                    }

                }
                db.SaveChanges();
            } 
            return services;
        }

        public static string[] GetPluginDirectories()
        {
            //return Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + @"\Plugins").ToList().Select(x=> Path.GetFileName(Path.GetDirectoryName(x))).ToArray();
            return System.IO.Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"), "*", System.IO.SearchOption.AllDirectories).ToList().Select(x=> x.Substring(x.LastIndexOf(Path.DirectorySeparatorChar)+1)).ToArray();
        }


    }
}
