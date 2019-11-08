using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderWeb.Core.DataBase;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderWeb.DAL.Data.Configuration
{
    class DishConfiguration : IEntityTypeConfiguration<Dish>
    {
        public void Configure(EntityTypeBuilder<Dish> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Comment).IsRequired().HasMaxLength(300);
            builder.HasOne(p => p.Organization)
                .WithMany(t => t.Dishes)
                .HasForeignKey(p => p.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict); 

        }

    }
}
