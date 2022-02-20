using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Foreman.Client.Services
{
    public class PluginService
    {
        private HttpClient _httpClient;
        public PluginService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Foreman.ServerAPI.Public");
        }
        public async Task<byte[]> GetPluginByName(string name)
        {
            var dll = await _httpClient.GetFromJsonAsync<byte[]>($"Plugin/GetByName/" + name);
            return dll;
        }
        public async Task<string[]> GetPluginNames()
        {
            return await _httpClient.GetFromJsonAsync<string[]>($"Plugin/PluginNames");
        }
        public async Task<Foreman.Shared.Data.Plugin.Plugin[]> GetPlugins()
        {
            return await _httpClient.GetFromJsonAsync<Foreman.Shared.Data.Plugin.Plugin[]>($"Plugin/GetPlugins");
        }
        public async Task<HttpResponseMessage> Uninstall(int id)
        {
            return await _httpClient.GetAsync($"Plugin/Delete/{id}");
        }
        public async Task<HttpResponseMessage> Install(HttpContent file)
        {
            return await _httpClient.PostAsync($"Plugin/Install", file);
        }
        public async Task<HttpResponseMessage> GetPlugin(int id)
        {
            return await _httpClient.GetAsync($"Plugin/GetById/{id}");
        }
    }
}
