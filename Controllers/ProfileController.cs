using DriveForum.DatabaseContext;
using DriveForum.Models;
using DriveForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize()]
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
        [Authorize()]
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
        [Authorize()]
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
        [Authorize()]
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

        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> BanProfile(int userid)
        {
            User? user = await _context.Users.Include(u => u.UserPosts).Include(u => u.UserComments).Where(u => u.Id == userid).FirstOrDefaultAsync();
            if (user is not null && user.IsBanned == false)
            {
                user.IsBanned = true;
                if (user.UserComments is not null)
                {
                    foreach (var comment in user.UserComments)
                    {
                        comment.IsHidden = true;
                    }
                }
                if (user.UserPosts is not null)
                {
                    foreach (var post in user.UserPosts)
                    {
                        post.IsModerated = false;
                    }
                }
            }
            else if (user is not null && user.IsBanned == true)
            {
                user.IsBanned = false;
                if (user.UserComments is not null)
                {
                    foreach (var comment in user.UserComments)
                    {
                        comment.IsHidden = false;
                    }
                }
                if (user.UserPosts is not null)
                {
                    foreach (var post in user.UserPosts)
                    {
                        post.IsModerated = true;
                    }
                }
            }
            await _context.SaveChangesAsync();
            return Redirect($"../users/{user.Login}");
        }

        [Authorize()]
        public async Task<IActionResult> ChangePhoto(int userId, IFormFile photoUrl)
        {
            User? user = await _context.Users.FindAsync(userId);
            if (user != null && photoUrl != null)
            {
                string extension = Path.GetExtension(photoUrl.FileName).ToLowerInvariant();
                if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                {
                    TempData["ErrorMessage"] = "Можно загружать только PNG, JPG, JPEG.";
                    return Redirect($"../users/{user.Login}");
                }
                if (photoUrl.Length > 10 * 1024 * 1024)
                {
                    TempData["ErrorMessage"] = "Размер файла не может преышать 10 Мб.";
                    return Redirect($"../users/{user.Login}");
                }
                string fileName = $"user_photo_{Guid.NewGuid()}_{Path.GetFileName(photoUrl.FileName)}";
                string filePath = Path.Combine("wwwroot/images", fileName);
                string relativeFilePath = Path.Combine("/images", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photoUrl.CopyToAsync(stream);
                }
                user.PhotoUrl = relativeFilePath;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return Redirect($"../users/{user.Login}");
            }
            else 
            {
                return Redirect("/");
            }
        }
    }
}
