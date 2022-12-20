using Azure.Identity;
using Bodil.Services;
using Bodil.Services.TableStorage;
using Bodil.States;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add configuration
//var keyVaultUri = Environment.GetEnvironmentVariable("AZURE_KEYVAULT_URI") ?? throw new ArgumentNullException("Missing key vault URI");
var keyVaultUri = "https://kv-bodil.vault.azure.net/";

var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile("appsettings.Development.json", true, true)
        .AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential())
.Build();

builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());

// Add services to the container.

var initialScopes = config["DownstreamApi:Scopes"]?.Split(' ');

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(config.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
            .AddMicrosoftGraph(config.GetSection("DownstreamApi"))
            .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages();
builder.Services.AddMudServices();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler()
    .AddCircuitOptions(option => { option.DetailedErrors = true; });

//Data
builder.Services.AddSingleton<IReservationDataService, ReservationTableService>();
builder.Services.AddSingleton<IUserDataService, UserTableStorage>();
builder.Services.AddSingleton<TableClientFactory>();

// Misc
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserState>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();