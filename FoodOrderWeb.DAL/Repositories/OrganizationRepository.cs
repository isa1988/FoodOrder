using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.Core.Repositories;
using FoodOrderWeb.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderWeb.DAL.Repositories
{
    class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(DataDbContext context) : base(context)
        {
            DbSet = context.Organizations;
        }

        public override IEnumerable<Organization> GetAll()
        {
            return DbSet.Include(p => p.Dishes).ToList();
        }

        public override Organization GetById(int id)
        {
            return DbSet.Include(p => p.Dishes).FirstOrDefault(p => p.Id == id);
        }

        public bool IsExistByNAme(string name)
        {
            return DbSet.Any(p => p.Name == name);
        }

        public bool IsExistByNAme(int id, string name)
        {
            return DbSet.Any(p => p.Id != id && p.Name == name);
        }

        public bool IsHasDishesh(int id)
        {
            return DbSet.Include(p => p.Dishes).FirstOrDefault(p => p.Id == id)?.Dishes?.Count>0;
        }
    }
}
