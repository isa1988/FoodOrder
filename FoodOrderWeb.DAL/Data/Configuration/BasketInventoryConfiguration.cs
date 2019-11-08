using FoodOrderWeb.Core.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.DAL.Data.Configuration
{
    class BasketInventoryConfiguration : IEntityTypeConfiguration<BasketInventory>
    {
        public void Configure(EntityTypeBuilder<BasketInventory> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Basket)
                .WithMany(t => t.BasketInventories)
                .HasForeignKey(p => p.BasketId)
                .OnDelete(DeleteBehavior.Restrict); ;

        }
    }
}
