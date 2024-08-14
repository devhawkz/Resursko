using System.Net.Http.Headers;
using Toolbelt.Blazor;

namespace Resursko.Client.Respositories.HttpRespository;

public class HttpInterceptorService(HttpClientInterceptor interceptor, RefreshTokenService tokenService)
{
    public void RegisterEvent() => interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

    public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
    {
        var absPath = e.Request.RequestUri!.AbsolutePath;
        if (!absPath.Contains("token") && !absPath.Contains("accounts"))
        {
            var token = await tokenService.TryRefreshToken();
            if (!string.IsNullOrEmpty(token))
            {
                e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            }
        }
    }

    public void DisposeEvent() => interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
}
