using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Foreman.Shared.Models.Category;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Collections.Generic;

namespace Foreman.Client.Services
{
    public class CourseService
    {
        private HttpClient _httpClient;

        public CourseService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Foreman.ServerAPI");
        }
        public async Task<HttpResponseMessage> CreateCategory(CategoryModel categoryModel)
        {
            var apiResult = await _httpClient.PostAsJsonAsync<CategoryModel>($"Course/CreateCategory", categoryModel);
            return apiResult;
        }
        public async Task<HttpResponseMessage> EditCategory(CategoryModel categoryModel)
        {
            var apiResult = await _httpClient.PostAsJsonAsync<CategoryModel>($"Course/EditCategory", categoryModel);
            return apiResult;
        }
        public async Task<HttpResponseMessage> DeleteCategory(int categoryId)
        {
            var apiResult = await _httpClient.PostAsJsonAsync<int>($"Course/CreateCategory", categoryId);
            return apiResult;
        }
        public async Task<HttpResponseMessage> SearchCategory(string search)
        {
            var apiResult = await _httpClient.PostAsJsonAsync<string>($"Course/SearchCategory", search);
            return apiResult;
        }
    }
}    
