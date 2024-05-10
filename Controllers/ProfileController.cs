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
            User? user = await _context
                .Users
                .Where(x => x.Login == login)
                .Include(u => u.UserPosts)
                .Include(u => u.Cars)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                user.UserPosts = user.UserPosts.Select(post =>
                {
                    if (post.Main?.Length > 130)
                    {
                        post.Main = $"{post.Main.Substring(0, 130)}...";
                    }
                    return post;
                }).ToList();

                List<Car>? cars = await _context
                    .Cars
                    .Include(c => c.Engine)
                    .Include(c => c.Model.Brand)
                    .ToListAsync();

                return View(new UserProfileVM()
                {
                    User = user,
                    Cars = cars
                });
            }

            return NotFound();
        }


        public async Task<IActionResult> ChangeDesc(int id, string? description)
        {
            User? user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user != null && description?.Length <= 100 || string.IsNullOrEmpty(description))
            {
                user.Description = description;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return Redirect($"../users/{user.Login}");
            }
            return Redirect($"../users/{user.Login}");
        }
        [Route("users/{login}/cars")]
        public async Task<IActionResult> MyCars(string login)
        {
            User? user = await _context
                .Users
                .Where(x => x.Login == login)
                .Include(u => u.Cars)
                .FirstOrDefaultAsync();
            List<Car>? cars = await _context
                .Cars
                .Include(c => c.Engine)
                .Include(c => c.Model.Brand)
                .OrderBy(c => c.Model.Brand.Name)
                .ToListAsync();
            return View(new UserProfileVM()
            {
                User = user,
                Cars = cars
            });
        }
        public async Task<IActionResult> DeleteCar(int carid, int userid, string login)
        {
            UserCar? usercar = await _context.UserCars.Where(u => u.Car.Id == carid && u.User.Id == userid).FirstOrDefaultAsync();
            if (usercar != null)
            {
                _context.UserCars.Remove(usercar);
                await _context.SaveChangesAsync();
            }
            return Redirect($"../users/{login}/cars");
        }
        public async Task<IActionResult> AddCar(int carid, int userid, string login)
        {
            Car? newcar = await _context.Cars.FindAsync(carid);
            User? user = await _context.Users.FindAsync(userid);
            if (await _context.UserCars.AnyAsync(uc => uc.Car.Id == carid && uc.User.Id == userid))
            {
                TempData["ErrorMessage"] = "У вас уже есть такая машина!";
                return Redirect($"../users/{login}/cars");
            }
            if (await _context.UserCars.CountAsync(uc => uc.User.Id == userid) >= 5)
            {
                TempData["ErrorMessage"] = "Вы не можете иметь более 5 машин!";
                return Redirect($"../users/{login}/cars");
            }
            UserCar newusercar = new() { Car = newcar, User = user };
            _context.UserCars.Add(newusercar);
            await _context.SaveChangesAsync();
            return Redirect($"../users/{login}/cars");
        }
    }
}
