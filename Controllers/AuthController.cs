using DriveForum.DatabaseContext;
using DriveForum.Models;
using DriveForum.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DriveForum.Controllers
{
    public class AuthController : Controller
    {
        ApplicationContext _context;

        public AuthController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Registration()
        {
            return View();
        }

        private async Task Auth(User user)
        {
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<Claim> { new Claim(ClaimTypes.Name, user.Login), new Claim(ClaimTypes.Role, user.Role.ToString()) },
                    "Cookies"
                )));
        }

        public async Task<IActionResult> Register(AuthUser newuser)
        {
            if (ModelState.IsValid)
            {
                using SHA256 hash = SHA256.Create();
                var user = new User(
                    newuser.Username,
                    newuser.Email,
                    newuser.Login,
                    Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(newuser.Password))));
                if (await _context.Users.Where(u => u.Email == user.Email || u.Login == user.Login || u.Username == user.Username).FirstOrDefaultAsync() != null)
                {
                    ModelState.AddModelError("", "Такой логин/email/имя пользователя занято!");
                    return View("Registration");
                }
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return Redirect("../Auth/Login");
            }
            else
            {
                return View("Registration");
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Log_in(string login, string password)
        {
            using SHA256 hash = SHA256.Create();
            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password))
            {
                User? LoginUser = await _context.Users.Where(u => u.Login == login && u.Password == Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(password)))).FirstOrDefaultAsync();
                if (LoginUser != null)
                {
                    await Auth(LoginUser);
                    return Redirect("/");
                }
            }
            ModelState.AddModelError("", "Неправильный логин/пароль");
            return View("Login");
        }

        [Authorize()]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
