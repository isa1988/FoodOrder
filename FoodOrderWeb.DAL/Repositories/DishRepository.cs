using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.Core.Repositories;
using FoodOrderWeb.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FoodOrderWeb.DAL.Repositories
{
    public class DishRepository : Repository<Dish>, IDishRepository
    {
        public DishRepository(DataDbContext context) : base(context)
        {
            DbSet = context.Dishes;
        }

        public override IEnumerable<Dish> GetAll()
        {
            return DbSet.Include(p => p.Organization).ToList();
        }

        public IEnumerable<Dish> GetAll(int organizationId)
        {
            return DbSet.Include(p => p.Organization).Where(p => p.OrganizationId == organizationId).ToList();
        }

        public override Dish GetById(int id)
        {
            return DbSet.Include(p => p.Organization).FirstOrDefault(p => p.Id == id);
        }

        public bool IsExistByNAme(string name, int organizationId)
        {
            return DbSet.Any(p => p.Name == name && p.OrganizationId == organizationId);
        }

        public bool IsExistByNAme(int id, string name, int organizationId)
        {
            return DbSet.Any(p => p.Id != id && p.Name == name && p.OrganizationId == organizationId);
        }
    }
}

