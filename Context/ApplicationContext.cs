﻿using DriveForum.Models;
using Microsoft.EntityFrameworkCore;

namespace DriveForum.DatabaseContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<CarEngine> CarEngines { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPost> UserPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserCar> UserCars { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
