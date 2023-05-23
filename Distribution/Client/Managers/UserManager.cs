using Distribution.Shared;
using Microsoft.AspNetCore.Components.Routing;
using System.Net.Http.Json;

namespace Distribution.Client.Managers
{

    public interface IUserManager 
    {
        Task<IEnumerable<UserLoginDto>> GetAllAsync();
    }
    public class UserManager : IUserManager
    {
        private readonly HttpClient _httpClient;

        public UserManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UserLoginDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/User/");
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<UserLoginDto>>();
            return result;
        }
    }
}
