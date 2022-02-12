using MudBlazor;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Foreman.Client.Services
{
    public class AppStateService
    {
        private HttpClient _httpClient;

        public AppStateService(IHttpClientFactory httpFactory)
        {
            _httpClient = httpFactory.CreateClient("Foreman.ServerAPI");
        }

        public event Action OnChange;

        public List<BreadcrumbItem> Breadcrumbs { get; private set; }

        public AppStateService()
        {
            Breadcrumbs = new List<BreadcrumbItem>();
        }

        public async Task SetBreadcrumbs(List<BreadcrumbItem> items)
        {
            Breadcrumbs = items;
            OnChange?.Invoke(); ;
        }

        public async Task SetBreadcrumbsByCourseCategory(int id, bool isCourse, List<BreadcrumbItem> pushAdditional=null)
        {

            var result = await _httpClient.GetAsync($"/course/GetBreadcrumbs/{id}/{isCourse}");

            if (!result.IsSuccessStatusCode)
            {
                //TODO: Oblusga bledu
                return;
            }

            var bread = await result.Content.ReadFromJsonAsync<List<BreadcrumbItem>>();
            if(pushAdditional!=null)
                bread.AddRange(pushAdditional);
            Breadcrumbs = bread;
            OnChange?.Invoke();
        }

    }
}
