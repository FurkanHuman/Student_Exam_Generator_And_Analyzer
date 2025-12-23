using Application;
using BlazorWebUI;
using BlazorWebUI.Components;
using BlazorWebUI.Components.Account;
using BlazorWebUI.Extensions;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using NArchitecture.Core.CrossCuttingConcerns.Logging.Configurations;
using NArchitecture.Core.ElasticSearch.Models;
using NArchitecture.Core.Mailing;
using NArchitecture.Core.Persistence.WebApi;
using Persistence;
using Persistence.Contexts;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies(options =>
    {
        options.ApplicationCookie?.Configure(cookieOptions =>
        {
            cookieOptions.Cookie.Name = "SES_Credential";
        });
    });


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<PostgreSqlUserDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddBlazorWebUIServiceRegistration();
builder.Services.AddDistributedMemoryCache();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        string? certPath = builder.Configuration["ASPNETCORE_Kestrel__Certificates__Default__Path"];
        string? certPassword = builder.Configuration["ASPNETCORE_Kestrel__Certificates__Default__Password"];

        if (!string.IsNullOrEmpty(certPath) && !string.IsNullOrEmpty(certPassword))
        {
            httpsOptions.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath, certPassword);
        }
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(opt =>
{
    opt.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
    opt.KnownNetworks.Clear();
    opt.KnownProxies.Clear();
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}

else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDbMigrationApplier();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorWebUI.Client._Imports).Assembly)
    .DisableAntiforgery();

app.Use(async (context, next) =>
{
    if (string.IsNullOrEmpty(context.Request.Headers.AcceptLanguage))
    {
        context.Request.Headers.AcceptLanguage = "tr-TR";
    }
    await next();
});
app.MapAdditionalIdentityEndpoints();
app.MapZeroFileProxy();

await app.RunAsync();
