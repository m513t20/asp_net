using PersonalAccount.Common.Models;
using PersonalAccount.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
                    .AddJsonFile("connectionsettings.json")
                    .Build();
var options = configuration.GetSection(nameof(ApiOptions)).Get<ApiOptions>()
                        ?? throw new InvalidOperationException($"Невозможно загрузить настройки из секции {nameof(ApiOptions)}!");

builder.Services.RegistryPersonalAccountData( configuration );

builder.Services.AddScoped<PersonalAccount.Web.Interfaces.ISettingsStorage,
    PersonalAccount.Web.Storage.SettingsStorage>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
