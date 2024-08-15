using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Resursko.Client.AuthProviders;
using Resursko.Client.Respositories.HttpRespository;
using Resursko.Client.Services.Account;
using Resursko.Client.Services.Administrator;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Resursko.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            }
            .EnableIntercept(sp));

            builder.Services.AddHttpClientInterceptor();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddScoped<HttpInterceptorService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<RefreshTokenService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            
            await builder.Build().RunAsync();
        }
    }
}
