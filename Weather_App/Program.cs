using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Weather_App.Data;
using Weather_App.Services;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(builder.Configuration.GetSection("KeyVaultURL").Value!);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConnectionStrings--try46") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IPositionDataTransformations, PositionDataTransformations>();
builder.Services.AddSingleton<IPositionServiceHandler, PositionServiceHandler>();
builder.Services.AddSingleton<IWeatherDataTransformations, WeatherDataTransformations>();
builder.Services.AddSingleton<IWeatherServiceHandler, WeatherServiceHandler>();
builder.Services.AddSingleton<IWeatherService, WeatherService>();

builder.Services.AddResponseCaching();

builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
