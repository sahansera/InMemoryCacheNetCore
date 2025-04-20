using InMemoryCachingSample.Infrastructure;
using InMemoryCachingSample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();
// Register base services
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<ICacheProvider, CacheProvider>();
// Use fully qualified name to resolve ambiguity
builder.Services.AddScoped<IHttpClient, HttpClient>();
// Register IUsersService implementation with decorator pattern
builder.Services.AddScoped<IUsersService>(sp => 
{
    var usersService = sp.GetRequiredService<UsersService>();
    var cacheProvider = sp.GetRequiredService<ICacheProvider>();
    return new CachedUserService(usersService, cacheProvider);
});

// Configure HTTP strict transport security
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
    options.Preload = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();