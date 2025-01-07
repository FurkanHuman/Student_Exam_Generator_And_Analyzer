using BlazorWebUI.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddBlazorWebUIClientServiceRegistration();

await builder.Build().RunAsync();
