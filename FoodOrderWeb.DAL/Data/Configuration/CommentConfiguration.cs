using FoodOrderWeb.Core.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.DAL.Data.Configuration
{
    class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.User)
                .WithMany(t => t.Comments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.BasketInventory)
                .WithMany(t => t.Comments)
                .HasForeignKey(p => p.BasketInventoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(p => p.Text).IsRequired().HasMaxLength(300);
        }
    }
}
