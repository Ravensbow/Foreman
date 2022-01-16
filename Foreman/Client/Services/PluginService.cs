using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Foreman.Client.Services
{
    public class PluginService
    {
        private readonly HttpClient _httpClient;
        public PluginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<byte[]> GetPluginByName(string name)
        {
            var temp = await _httpClient.GetByteArrayAsync($"Plugin/GetByName/" + name);
            return temp;
        }
        public async Task<string[]> GetPluginNames()
        {
            return await _httpClient.GetFromJsonAsync<string[]>($"Plugin/PluginNames");
        }
    }
}
