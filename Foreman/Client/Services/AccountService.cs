using Foreman.Shared.Models.Account;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Client.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;
        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> Login(LoginModel loginModel)
        {
            HttpContent c = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");
            var apiResponse = await _httpClient.PostAsync($"Account/Login", c);
            if(apiResponse.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        public async Task<bool> Register(RegisterModel registerModel)
        {
            HttpContent c = new StringContent(JsonConvert.SerializeObject(registerModel), Encoding.UTF8, "application/json");
            var apiResponse = await _httpClient.PostAsync($"Account/Register", c);
            if (apiResponse.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
    }
}
