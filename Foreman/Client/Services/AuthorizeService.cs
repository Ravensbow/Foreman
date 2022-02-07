using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Foreman.Client.Services
{
    public class AuthorizeService
    {

        private readonly HttpClient _httpClient;

        public AuthorizeService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Foreman.ServerAPI");
        }
        
        public async Task<bool> CanViewCategory(int categoryId)
        {
            return await _httpClient.GetFromJsonAsync<bool>($"Auhtorize/CanViewCategory?categoryId={categoryId}");
        }

        public async Task<bool> CanViewCourse(int courseId)
        {
            return await _httpClient.GetFromJsonAsync<bool>($"Authorize/CanViewCourse?courseId={courseId}");
        }

        public async Task<bool> CanEditCourse(int courseId)
        {
            return await _httpClient.GetFromJsonAsync<bool>($"Authorize/CanEditCourse?courseId={courseId}");
        }
    }
}
