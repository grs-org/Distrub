using Blazored.LocalStorage;
using Distribution.Client;
using Distribution.Client.Authentification;
using Distribution.Client.Managers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Radzen.Blazor;

namespace Distribution.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<UniStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider, UniStateProvider>();
            builder.Services.AddScoped<IUserManager, UserManager>();
            builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<DialogService>();
            //builder.Services.AddScoped<RadzenNotification>();

            await builder.Build().RunAsync();
        }
    }
}