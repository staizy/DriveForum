﻿using DriveForum.DatabaseContext;
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

        public async Task<IActionResult> ChangeDesc(int id, string? description)
        {
            User? user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user != null && description != null && description.Length <= 100)
            {
                user.Description = description;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return Redirect($"../users/{user.Login}");

            }
            else if (description == null && description?.Length > 100)
            {
                ModelState.AddModelError("", "Слшиком длинное описание!");
                return View("ProfilePage");
            }
            return Redirect($"../users/{user.Login}");
        }
    }
}