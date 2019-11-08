using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderWeb.Core.DataBase;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderWeb.DAL.Data.Configuration
{
    class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Comment).IsRequired().HasMaxLength(200);
        }

    }
}