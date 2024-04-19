using DriveForum.DatabaseContext;
using DriveForum.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DriveForum.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DriveForum.Controllers
{
    public class ProfileController : Controller
    {
        ApplicationContext _context;

        public ProfileController(ApplicationContext context)
        {
            _context = context;
        }

        [Route("/users/{login}")]
        public async Task<IActionResult> ProfilePage(string login)
        {
            User? user = await _context.Users.Where(u => u.Login == login).FirstOrDefaultAsync();
            if (user != null) return View(user);
            else return NotFound();
        }
    }
}
