using DriveForum.DatabaseContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
    AddCookie(option =>
    {
        option.LoginPath = new PathString("/Auth/Login");
        option.LogoutPath = new PathString("/Home/Registration");
    });
builder.Services.AddAuthorization();

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Registration}"
    );

app.Run();