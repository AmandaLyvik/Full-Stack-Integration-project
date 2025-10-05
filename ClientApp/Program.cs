using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClientApp;
using Blazored.SessionStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5086") });
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ConsoleLogger>();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<ISessionCacheService, SessionCacheService>();

await builder.Build().RunAsync();
