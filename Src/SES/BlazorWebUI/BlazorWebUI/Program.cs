using Application;
using BlazorWebUI;
using BlazorWebUI.Client;
using BlazorWebUI.Components;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using NArchitecture.Core.CrossCuttingConcerns.Logging.Configurations;
using NArchitecture.Core.ElasticSearch.Models;
using NArchitecture.Core.Localization.WebApi;
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

builder.Services.AddApplicationServices(
    mailSettings: builder.Configuration.GetSection("MailSettings").Get<MailSettings>()
        ?? throw new InvalidOperationException("MailSettings section cannot found in configuration."),
    fileLogConfiguration: builder
        .Configuration.GetSection("SeriLogConfigurations:FileLogConfiguration")
        .Get<FileLogConfiguration>()
        ?? throw new InvalidOperationException("FileLogConfiguration section cannot found in configuration."),
    elasticSearchConfig: builder.Configuration.GetSection("ElasticSearchConfig").Get<ElasticSearchConfig>()
        ?? throw new InvalidOperationException("ElasticSearchConfig section cannot found in configuration.")
);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddBlazorWebUIServiceRegistration();

builder.Services.AddDistributedMemoryCache();

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


app.UseResponseLocalization();
app.MapAdditionalIdentityEndpoints();
app.Run();
