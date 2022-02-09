using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Foreman.Shared.Models.Category;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Collections.Generic;


namespace Foreman.Client.Services
{
    public class InstitutionService
    {
        private HttpClient _httpClient;
        public InstitutionService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Foreman.ServerAPI");
        }

        public async Task<HttpResponseMessage> GetInstitutionForManager(int userId)
        {
            var apiResult = await _httpClient.PostAsJsonAsync<int>($"Institution/GetInstitutionForManager", userId);
            return apiResult;
        }
    }
}
