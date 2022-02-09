using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foreman.Client.Services
{
    public class AppStateService
    {
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


    }
}
