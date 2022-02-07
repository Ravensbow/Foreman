using Foreman.Shared.Models.Account;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Client.Services
{
    public class AccountService
    {
        private HttpClient _httpClient;
        public AccountService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Foreman.ServerAPI.Public");
        }
        public async Task<HttpResponseMessage> Login(LoginModel loginModel)
        {
            HttpContent c = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");
            var apiResponse = await _httpClient.PostAsync($"Account/Login", c);
            return apiResponse;
        }
        public async Task<HttpResponseMessage> Register(RegisterModel registerModel)
        {
            HttpContent c = new StringContent(JsonConvert.SerializeObject(registerModel), Encoding.UTF8, "application/json");
            var apiResponse = await _httpClient.PostAsync($"Account/Register", c);
            return apiResponse;
        }

        public async Task <HttpResponseMessage> ConfirmEmail(string code, int userid)
        {
            var apiResponse = await _httpClient.GetAsync($"Account/Register/"+userid+"/"+code);
            return apiResponse;
        }
    }
}
