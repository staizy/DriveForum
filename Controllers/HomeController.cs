using DriveForum.DatabaseContext;
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
        [Route("/Error")]
        public IActionResult Error()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }
    }
}
