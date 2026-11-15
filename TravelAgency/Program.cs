
using TravelAgency.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 1. Register MVC and Web API
builder.Services.AddControllersWithViews();

// 2. Register repositories and services
builder.Services.AddTravelAgencyServices();

// 3. Register authorization
builder.Services.AddAuthorization();

// 4. Configure Session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);

    options.Cookie.Name = ".TravelAgency.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// 5. Build application
var app = builder.Build();

// 6. Configure error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 7. HTTPS redirection
app.UseHttpsRedirection();

// 8. Static files
app.UseStaticFiles();

// 9. Routing
app.UseRouting();

// 10. Session
app.UseSession();

// 11. Authorization
app.UseAuthorization();

// 12. API routes
app.MapControllers();

// 13. MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
