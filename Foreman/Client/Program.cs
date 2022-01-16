using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.Web;
using Foreman.Client.Services;
using Foreman.Client.Utilites;
using System.IO;
using System.Reflection;

namespace Foreman.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            
            builder.Services.AddHttpClient("Foreman.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<AccountService>(x => x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddHttpClient<PluginService>(x => x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Foreman.ServerAPI"));

            builder.Services.AddApiAuthorization();

            PluginService ps = builder.Services.BuildServiceProvider()
                    .GetService<PluginService>();

            var temp = await ps.GetPluginNames();
            List<byte[]>assemblyDatas = new List<byte[]>();
            foreach (var name in temp)
            {
                byte[] assemblyData = await ps.GetPluginByName(name);
                assemblyDatas.Add(assemblyData);
            }

            builder.Services.AddSingleton<IComponentService>( _ =>
            {
                var service = new ComponentService();
                //foreach (var component in assemblyDatas)
                //{
                //    Assembly.Load(component);
                //}
                service.LoadComponents(assemblyDatas);
                return service;
            });

            builder.Services.AddMudServices();
            await builder.Build().RunAsync();
        }
    }
}
