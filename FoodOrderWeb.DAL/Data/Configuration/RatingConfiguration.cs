using FoodOrderWeb.Core.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.DAL.Data.Configuration
{
    class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.User)
                .WithMany(t => t.Ratings)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.BasketInventory)
                .WithMany(t => t.Ratings)
                .HasForeignKey(p => p.BasketInventoryId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}