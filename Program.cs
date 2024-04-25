using DriveForum.DatabaseContext;
using DriveForum.Models;
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
builder.Services.AddScoped<Auth, Auth>();
builder.Services.AddHttpContextAccessor();

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Post}/{action=Feed}"
    );

app.Run();

public class Auth
{
    public User? User { get; set; } = null;
    public Auth(IHttpContextAccessor ctx, ApplicationContext db)
    {
        if (ctx.HttpContext?.User.Identity != null)
        {
            User = db.Users.Where(o => o.Login == ctx.HttpContext.User.Identity.Name).FirstOrDefault();
        }
    }
}