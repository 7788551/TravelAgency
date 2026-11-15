using TravelAgency.Extensions;

var builder = WebApplication.CreateBuilder(args);

// MVC + Web API
builder.Services.AddControllersWithViews();

// Register repositories and services
builder.Services.AddTravelAgencyServices();

// Session storage
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);

    options.Cookie.Name = ".TravelAgency.Session";

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;

    options.Cookie.SecurePolicy =
        CookieSecurePolicy.Always;

    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Session must run before endpoint execution
app.UseSession();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();