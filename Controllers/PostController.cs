using DriveForum.DatabaseContext;
using DriveForum.Models;
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
        public async Task<IActionResult> CreatePost()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(UserPost post)
        {
            int? carId = post?.Car?.Id;
            Car? car = await _context.Cars
                .Include(c => c.Engine)
                .Include(c=> c.Model)
                .Include(c=> c.Model.Brand)
                .Where(c=> c.Id == carId)
                .FirstOrDefaultAsync();
            return View();
        }
    }
}