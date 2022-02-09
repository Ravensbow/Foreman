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
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using System.Linq;

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
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("Foreman.ServerAPI"));

            builder.Services.AddHttpClient("Foreman.ServerAPI.Public", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddSingleton<AccountService>();      
            builder.Services.AddSingleton<PluginService>();
            builder.Services.AddSingleton<AuthorizeService>();
            builder.Services.AddSingleton<AppStateService>();
            builder.Services.AddSingleton<CourseService>();
            builder.Services.AddSingleton<InstitutionService>();

            builder.Services.AddApiAuthorization();
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("CanEditCourse", policy =>
                    policy.RequireAssertion(async context =>
                    {
                        if (context.Resource is RouteData rd)
                        {
                            var routeValue = rd.RouteValues.TryGetValue("courseId", out var value);
                            var id = Convert.ToInt32(value);
                            //var test = builder.Services.BuildServiceProvider().GetService<IHttpClientFactory>().CreateClient("Foreman.ServerAPI");
                            //var r = test.GetFromJsonAsync<bool>($"Authorize/CanEditCourse?courseId={id}");
                            AuthorizeService  authService = builder.Services.BuildServiceProvider()
                                .GetService<AuthorizeService>();
                            try
                            {
                                return await authService.CanEditCourse(id);
                            }
                            catch (AccessTokenNotAvailableException ex)
                            {
                                ex.Redirect();
                            }
                        }
                        return false;
                    })
                );
            });

            PluginService ps = builder.Services.BuildServiceProvider()
                    .GetService<PluginService>();

            var temp = await ps.GetPluginNames();
            List<byte[]> assemblyDatas = new List<byte[]>();
            foreach (var name in temp)
            {
                byte[] assemblyData = await ps.GetPluginByName(name);
                assemblyDatas.Add(assemblyData);
            }

            builder.Services.AddSingleton<IComponentService>(_ =>
           {
               var service = new ComponentService();
               service.LoadComponents(assemblyDatas);
               return service;
           });

            builder.Services.AddMudServices();
            await builder.Build().RunAsync();
        }
    }
}
