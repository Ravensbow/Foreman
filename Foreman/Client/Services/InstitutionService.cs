using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Foreman.Shared.Models.Category;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Collections.Generic;
using Foreman.Shared.Data.Identity;

namespace Foreman.Client.Services
{
    public class InstitutionService
    {
        private HttpClient _httpClient;
        public InstitutionService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Foreman.ServerAPI");
        }

        public async Task<HttpResponseMessage> GetPotentialInstitutionManagers()
        {
            var apiResult = await _httpClient.GetAsync($"Institution/GetPotentialInstitutionManagers");
            return apiResult;
        }

        public async Task<HttpResponseMessage> GetInstitutionForManager(int userId)
        {
            var apiResult = await _httpClient.GetAsync($"Institution/GetInstitutionForManager/{userId}");
            return apiResult;
        }

        public async Task<HttpResponseMessage> GetInstitution(int institutionId)
        {
            var apiResult = await _httpClient.GetAsync($"Institution/GetInstitution/{institutionId}");
            return apiResult;
        }

        public async Task<HttpResponseMessage> CreateInstitution(Institution model)
        {
            var apiResult = await _httpClient.PostAsync("Institution/CreateInstitution", new StringContent(JsonConvert.SerializeObject(model, settings:
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None }), System.Text.Encoding.UTF8, "application/json"));
            return apiResult;
        }

        public async Task<HttpResponseMessage> EditInstitution(Institution model)
        {
            var apiResult = await _httpClient.PostAsync("Institution/UpdateInstitution", new StringContent(JsonConvert.SerializeObject(model, settings:
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None }), System.Text.Encoding.UTF8, "application/json"));
            return apiResult;
        }
    }
}
