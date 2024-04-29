using DriveForum.DatabaseContext;
using DriveForum.Models;
using DriveForum.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DriveForum.Controllers
{
    public class PostController : Controller
    {
        ApplicationContext _context;

        public PostController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Feed()
        {
            return View(_context?.UserPosts?
                .Include(u => u.User)
                .Include(c => c.Car.Model.Brand)
                .Include(c => c.Car.Engine)
                .Where(u => u.IsModerated == false)
                .ToList());
        }

        [HttpGet]
        public async Task<IActionResult> CreatePost()
        {
            return View(new CarsAndUserPost()
            {
                CarBrands = _context.CarBrands.ToList(),
                CarEngines = _context.CarEngines.ToList(),
                CarModels = _context.CarModels.ToList(),
                Cars = _context.Cars.ToList(),
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(int carId, int userId, string title, string main, IFormFile? image)
        {
            User? user = await _context.Users
                .FindAsync(userId);
            Car? car = await _context.Cars
                .Include(c => c.Engine)
                .Include(c => c.Model)
                .Include(c => c.Model.Brand)
                .Where(c=> c.Id == carId)
                .FirstOrDefaultAsync();
            UserPost newpost = new()
            {
                Car = car,
                User = user,
                Title = title,
                Main = main
            };
            if (user != null && newpost != null && car != null)
            {
                await _context.UserPosts.AddAsync(newpost);
                await _context.SaveChangesAsync();
            }
            return Redirect($"../users/{user.Login}");
        }
    }
}