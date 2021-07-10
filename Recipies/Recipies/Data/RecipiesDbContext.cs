﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipies.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recipies.Data
{
    public class RecipiesDbContext : IdentityDbContext
    {
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Recipe> Recipes { get; set; }
        public RecipiesDbContext(DbContextOptions<RecipiesDbContext> options)
            : base(options)
        {
        }
    }
}
