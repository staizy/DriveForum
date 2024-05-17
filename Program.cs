using DriveForum.DatabaseContext;
using DriveForum.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
    AddCookie(option =>
    {
        option.LoginPath = new PathString("/Auth/Login");
        option.LogoutPath = new PathString("/Home/Registration");
        option.AccessDeniedPath = new PathString("/Error");
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<Auth, Auth>();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllerRoute("default", "{controller=post}/{action=feed}/{id:int?}");

app.Run();