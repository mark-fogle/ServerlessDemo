using Api.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var appSettings = builder.Configuration.Get<AppSettings>();

builder.Services.AddHttpClient<IApiClient, ApiClient>(c =>
{
    var apiSettingsBaseUrl = appSettings?.ApiSettings?.BaseUrl;
    if (apiSettingsBaseUrl == null)
        throw new ArgumentNullException();
    c.BaseAddress = new Uri(new Uri(apiSettingsBaseUrl), "/api/Weather");
});

await builder.Build().RunAsync();

public record AppSettings
{
    public Api? ApiSettings { get; set; }

    public record Api
    {
        public string? BaseUrl { get; set; }
    }
}
