using Blazored.LocalStorage;
using Distribution.Shared.Requests;
using Distribution.Shared.Response;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Distribution.Client.Authentification
{
    public interface IAuthenticationManager 
    {
        //Task<IResult> Login(TokenRequest model);

        Task<string> Logout();

        Task<string> RefreshToken();

        Task<string> TryRefreshToken();

        Task<string> TryForceRefreshToken();

        Task<ClaimsPrincipal> CurrentUser();
        Task<string> GetToken();
    }
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationManager(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider
           )
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }


        public async Task<string> Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            ((UniStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return "Success";
        }
        public async Task<ClaimsPrincipal> CurrentUser()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User;
        }

        public async Task<string> GetToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken"); 
            return token;
        }

        

       

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

            var response = await _httpClient.PostAsJsonAsync("api/login/refresh", new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });

            var result = await response.ReadAsync<TokenResponse>();

            if (!result.Message.Equals("Success"))
            {
                throw new ApplicationException("Something went wrong during the refresh token action");
            }

            token = result.Token;
            refreshToken = result.RefreshToken;
            await _localStorage.SetItemAsync("authToken", token);
            await _localStorage.SetItemAsync("refreshToken", refreshToken);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return token;
        }

        public async Task<string> TryRefreshToken()
        {
            //check if token exists
            var availableToken = await _localStorage.GetItemAsync<string>("refreshToken");
            if (string.IsNullOrEmpty(availableToken)) return string.Empty;
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;
            if (diff.TotalMinutes <= 1)
                return await RefreshToken();
            return string.Empty;
        }

        public async Task<string> TryForceRefreshToken()
        {
            return await RefreshToken();
        }
    }
}