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
    public class CourseService
    {
        private HttpClient _httpClient;

        public CourseService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Foreman.ServerAPI");
        }
        
        public async Task<HttpResponseMessage> GetCategory(int categoryId)
        {
            var apiResult = await _httpClient.GetAsync($"Course/GetCategory/{categoryId}");
            return apiResult;
        }

        public async Task<HttpResponseMessage> GetCourse(int courseId)
        {
            var apiResult = await _httpClient.GetAsync($"Course/GetCourseById/{courseId}");
            return apiResult;
        }

        public async Task<HttpResponseMessage> CreateCourse(Foreman.Shared.Data.Courses.Course model)
        {
            var apiResult = await _httpClient.PostAsync("Course/CreateCourse", new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json"));
            return apiResult;
        }

        public async Task<HttpResponseMessage> UpdateCourse(Foreman.Shared.Data.Courses.Course model)
        {
            var apiResult = await _httpClient.PostAsync("Course/UpdateCourse", new StringContent(JsonConvert.SerializeObject(model, settings: 
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None }), System.Text.Encoding.UTF8, "application/json"));
            return apiResult;
        }

        public async Task<HttpResponseMessage> CreateCategory(Foreman.Shared.Data.Courses.CourseCategory categoryModel)
        {
            var apiResult = await _httpClient.PostAsJsonAsync<Foreman.Shared.Data.Courses.CourseCategory>($"Course/CreateCategory", categoryModel);
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
            var apiResult = await _httpClient.GetAsync($"Course/SearchCategory{(string.IsNullOrEmpty(search)?"":"/"+search)}");
            var temp = await apiResult.Content.ReadAsStringAsync();
            return apiResult;
        }
        public async Task<HttpResponseMessage> RemoveModule(int id)
        {
            var apiResult = await _httpClient.GetAsync($"Course/RemoveModule/{id}");
            return apiResult;
        }
    }
}    
