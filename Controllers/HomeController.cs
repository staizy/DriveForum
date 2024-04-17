using DriveForum.DatabaseContext;
using DriveForum.Models;
using Microsoft.AspNetCore.Mvc;

namespace DriveForum.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult MainPage()
        {
            return View();
        }
    }
}
