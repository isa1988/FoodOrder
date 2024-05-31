using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.DAL.Data.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderWeb.DAL.Data
{
    public class DataDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketInventory> BasketInventories { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DataDbContext(DbContextOptions<DataDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
            modelBuilder.ApplyConfiguration(new DishConfiguration());
            modelBuilder.ApplyConfiguration(new BasketInventoryConfiguration());
            modelBuilder.ApplyConfiguration(new BasketConfiguration());
            modelBuilder.ApplyConfiguration(new RatingConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}
