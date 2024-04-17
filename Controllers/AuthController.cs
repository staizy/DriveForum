using DriveForum.DatabaseContext;
using DriveForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        /*public async Task<IActionResult> Register(string login, string password, string email, string username)
        {
            if (ModelState.IsValid)
            {
                User user = new User(username, email, login, password);
                _context.Users.Add(user);
                _context.SaveChanges();
                return Redirect("../Home/MainPage");
            }
            else
            {
                return View("Registration");
            }
        }*/

        public async Task<IActionResult> Register(User newuser)
        {
            if (ModelState.IsValid)
            {
                var user = new User(newuser.Username, newuser.Email, newuser.Login, newuser.Password);
                _context.Users.Add(user);
                _context.SaveChanges();
                return Redirect("../Home/MainPage");
            }
            else
            {
                return View("Registration");
            }
        }

        public IActionResult Login(string login, string password)
        {
            //if ()
            return View();
        }

        public async Task<IActionResult> Log_in()
        {
            return BadRequest();
        }
    }
}
