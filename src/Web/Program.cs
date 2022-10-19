using Api.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var appSettings = builder.Configuration.Get<AppSettings>();
var apiSettingsBaseUrl = appSettings?.ApiSettings?.BaseUrl ?? "";
var apiClientId = appSettings?.ApiSettings?.ClientId ?? "";
var apiScope = appSettings?.ApiSettings?.Scope ?? "";
var apiTenantId = appSettings?.ApiSettings?.TenantId ?? "";

builder.Services.AddMsalAuthentication(options =>
{
    options.ProviderOptions.Authentication.Authority = $"https://login.microsoftonline.com/{apiTenantId}";
    options.ProviderOptions.Authentication.ClientId = apiClientId;
    options.ProviderOptions.Authentication.ValidateAuthority = true;
    options.ProviderOptions.DefaultAccessTokenScopes.Add(apiScope);
    options.ProviderOptions.LoginMode = "redirect";
});
builder.Services.AddScoped<AuthorizationMessageHandler>();
builder.Services.AddHttpClient<IApiClient, ApiClient>(c =>
    {

        if (apiSettingsBaseUrl == null)
            throw new ArgumentNullException();
        c.BaseAddress = new Uri(new Uri(apiSettingsBaseUrl), "/api/Weather");
    })
    .AddHttpMessageHandler(p =>
{
    var s = p.GetRequiredService<AuthorizationMessageHandler>();
    s.ConfigureHandler(new[] { apiSettingsBaseUrl }, new[] { apiScope });
    return s;
});

await builder.Build().RunAsync();

public record AppSettings
{
    public Api? ApiSettings { get; set; }

    public record Api
    {
        public string? BaseUrl { get; set; }
        public string? ClientId { get; set; }
        public string? Scope { get; set; }
        public string? TenantId { get; set; }
    }
}

