using FoodOrderWeb.DAL.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.DAL.Data
{
    public class DataDbContextFactory : IDataDbContextFactory
    {
        private readonly DbContextOptions<DataDbContext> _options;

        public DataDbContextFactory(
            DbContextOptions<DataDbContext> options)
        {
            _options = options;
        }

        public DataDbContext Create()
        {
            return new DataDbContext(_options);
        }
    }
}