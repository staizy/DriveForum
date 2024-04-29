using DriveForum.DatabaseContext;
using DriveForum.Models;
using DriveForum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public async Task<IActionResult> ProfilePage(string login)
        {
            /*User? user = await _context.Users
                .Where(x => x.Login == login)
                .FirstOrDefaultAsync();
            List<UserCar> usercars = await _context.UserCars
                .Where(u => u.User.Id == user.Id).ToListAsync();
            List<Car> cars = await _context.Cars
                .Include(u=> u.Engine)
                .Include(u=> u.Model.Brand)
                .ToListAsync();
            List<UserPost> userposts = await _context.UserPosts
                .Where(u => u.User.Id == user.Id)
                .ToListAsync();*/

            /*User? user = await _context.Users
            .Where(x => x.Login == login)
            .Include(u => u.UserPosts)
            .Include(u => u.Cars)
            .FirstOrDefaultAsync();
            List<Car>? cars = await _context.Cars
                .Include(c => c.Engine)
                .Include(c => c.Model.Brand)
                .ToListAsync();
            if (user != null) return View(user);
            else return NotFound();*/
            User? user = await _context
                .Users
                .Where(x => x.Login == login)
                .Include(u => u.UserPosts)
                .Include(u => u.Cars)
                .FirstOrDefaultAsync();
            List<Car>? cars = await _context
                .Cars
                .Include(c => c.Engine)
                .Include(c => c.Model.Brand)
                .ToListAsync();
            if (user != null)
            {
                return View(new UserProfileVM()
                {
                    User = user,
                    Cars = cars
                });
            }
            else return NotFound();
        }

        public async Task<IActionResult> ChangeDesc(int id, string? description)
        {
            User? user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user != null && description?.Length <= 150 || string.IsNullOrEmpty(description))
            {
                user.Description = description;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return Redirect($"../users/{user.Login}");

            }
            return Redirect($"../users/{user.Login}");
        }
    }
}
