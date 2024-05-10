﻿using DriveForum.DatabaseContext;
using DriveForum.Models;
using DriveForum.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace DriveForum.Controllers
{
    public class PostController : Controller
    {
        ApplicationContext _context;

        public PostController(ApplicationContext context)
        {
            _context = context;
        }

        /*[HttpGet]
        [Route("feed")]
        public ActionResult Feed()
        {
            return View(_context?.UserPosts?
                .Include(u => u.User)
                .Include(c => c.Car.Model.Brand)
                .Include(c => c.Car.Engine)
                .Where(u => u.IsModerated == false)
                .ToList());
        }*/

        [HttpGet]
        [Route("feed")]
        public ActionResult Feed()
        {
            var posts = _context?.UserPosts?
        .Include(u => u.User)
        .Include(c => c.Car.Model.Brand)
        .Include(c => c.Car.Engine)
        .Where(u => u.IsModerated == false)
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
        [Route("post/createpost")]
        public async Task<IActionResult> CreatePost(int carId, int userId, string title, string main, IFormFile? image)
        {
            User? user = await _context.Users
                .FindAsync(userId);
            Car? car = await _context.Cars
                .Include(c => c.Engine)
                .Include(c => c.Model)
                .Include(c => c.Model.Brand)
                .Where(c => c.Id == carId)
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
        [Route("post/{postid}")]
        public async Task<IActionResult> DetailedPostWithComments(int postid)
        {
            UserPost? userpost = await _context.UserPosts
                .Include(u => u.User)
                .Include(c => c.Car.Model.Brand)
                .Include(c => c.Car.Engine)
                .Include(c => c.Comments).ThenInclude(c=>c.User)
                .Where(u => u.IsModerated == false)
                .Where(u => u.Id == postid).FirstOrDefaultAsync();
            return View(userpost);
        }
        [Route("addcomment")]
        public async Task<IActionResult> AddComment(int postid, int userid, string commentbody)
        {
            User? user = await _context.Users.FindAsync(userid);
            UserPost? userpost = await _context.UserPosts.FindAsync(postid);
            Comment newcomment = new() { Context = commentbody, User = user };
            userpost.Comments.Add(newcomment);
            await _context.SaveChangesAsync();
            return Redirect($"post/{postid}");
        }
    }
}