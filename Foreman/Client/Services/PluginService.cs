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
    }
}
