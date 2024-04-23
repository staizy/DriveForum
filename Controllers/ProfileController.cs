﻿using DriveForum.DatabaseContext;
using DriveForum.Models;
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
        public async Task<IActionResult> ProfilePage(string login)
        {
            User? user = await _context.Users.Where(u => u.Login == login).FirstOrDefaultAsync();
            if (user != null) return View(user);
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
            else if (description?.Length > 150)
            {
                ModelState.AddModelError("", "Описание не может содержать более 150 символов!");
                return View("ProfilePage", user);
            }
            return Redirect($"../users/{user.Login}");
        }
        [HttpGet]
        public async Task<IActionResult> ShowUserPosts(int userId)
        {
            if (userId != null)
            {
                List<UserPost> userPosts = await _context.UserPosts
                    .Include(u => u.User)
                    .Include(c => c.Car)
                    .Include(c => c.Car.Engine)
                    .Include(c => c.Car.Model)
                    .Where(u => u.User.Id == userId).ToListAsync();
            }
            return View();
        }
    }
}
