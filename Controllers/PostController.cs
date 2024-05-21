using DriveForum.DatabaseContext;
using DriveForum.Models;
using DriveForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
            var posts = _context?.UserPosts?
        .Include(u => u.User)
        .Include(c => c.Car.Model.Brand)
        .Include(c => c.Car.Engine)
        .Where(u => u.IsModerated == true)
        .ToList();

            foreach (var post in posts)
            {
                if (post.Main != null && post.Main.Length > 130)
                {
                    post.Main = $"{post.Main.Substring(0, 130)}...";
                }
            }
            return View(posts);
        }
        [HttpGet]
        [Authorize()]
        [Route("post/createpost")]
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
        [Authorize()]
        [Route("post/createpost")]
        public async Task<IActionResult> CreatePost(int carId, int userId, string title, string main, IFormFile? mainPhoto)
        {
            if (title == null || main == null)
            {
                TempData["ErrorMessage"] = "Заголовок или основной текст не могут быть пустыми!";
                return Redirect("createpost");
            }
            User? user = await _context.Users
                .FindAsync(userId);
            Car? car = await _context.Cars
                .Include(c => c.Engine)
                .Include(c => c.Model)
                .Include(c => c.Model.Brand)
                .Where(c => c.Id == carId)
                .FirstOrDefaultAsync();
            string filePath = "";
            string relativeFilePath = "";
            if (user != null && car != null)
            {
                if (mainPhoto != null)
                {
                    string extension = Path.GetExtension(mainPhoto.FileName).ToLowerInvariant();
                    if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                    {
                        TempData["ErrorMessage"] = "Можно загружать только PNG, JPG, JPEG.";
                        return Redirect("createpost");
                    }
                    if (mainPhoto.Length > 10 * 1024 * 1024)
                    {
                        TempData["ErrorMessage"] = "Размер файла не может преышать 10 Мб.";
                        return Redirect("createpost");
                    }
                    string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(mainPhoto.FileName)}";
                    filePath = Path.Combine("wwwroot/images", fileName);
                    relativeFilePath = Path.Combine("/images", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await mainPhoto.CopyToAsync(stream);
                    }
                }
            }
            UserPost newpost = new()
            {
                Car = car,
                User = user,
                Title = title,
                Main = main,
                MainPhotoUrl = relativeFilePath
            };
            if (newpost != null)
            {
                await _context.UserPosts.AddAsync(newpost);
                await _context.SaveChangesAsync();
            }
            return Redirect($"../users/{user.Login}");
        }
        [Route("/post/{postid}")]
        public async Task<IActionResult> DetailedPostWithComments(int postid)
        {
            UserPost? userpost = await _context.UserPosts
                .Include(u => u.User)
                .Include(c => c.Car.Model.Brand)
                .Include(c => c.Car.Engine)
                .Include(c => c.Comments).ThenInclude(c => c.User)
                .Where(u => u.Id == postid).FirstOrDefaultAsync();
            return View(userpost);
        }
        [Authorize()]
        [Route("addcomment")]
        public async Task<IActionResult> AddComment(int postid, int userid, string commentbody)
        {
            User? user = await _context.Users.FindAsync(userid);
            UserPost? userpost = await _context.UserPosts.FindAsync(postid);
            Comment newcomment = new() { Context = commentbody, User = user };
            userpost.Comments.Add(newcomment);
            await _context.SaveChangesAsync();
            return Redirect($"/post/{postid}");
        }
        [Authorize(Roles = "Moderator")]
        [Route("visiblepost")]
        public async Task<IActionResult> VisiblePost(int postid)
        {
            UserPost? currpost = await _context.UserPosts.FindAsync(postid);
            currpost.IsModerated = !currpost.IsModerated;
            await _context.SaveChangesAsync();
            return Redirect($"../post/{postid}");
        }
        [Authorize(Roles = "Moderator")]
        [Route("visiblecomment")]
        public async Task<IActionResult> VisibleComment(int commentid, int postid)
        {
            Comment? currcomment = await _context.Comments.FindAsync(commentid);
            currcomment.IsHidden = !currcomment.IsHidden;
            await _context.SaveChangesAsync();
            return Redirect($"../post/{postid}");
        }
    }
}