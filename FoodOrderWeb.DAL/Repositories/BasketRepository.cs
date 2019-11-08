using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.Core.Repositories;
using FoodOrderWeb.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderWeb.DAL.Repositories
{
    public class BasketRepository : Repository<Basket>, IBasketRepository
    {
        public BasketRepository(DataDbContext context) : base(context)
        {
            DbSet = context.Baskets;
        }

        public override IEnumerable<Basket> GetAll()
        {
            return DbSet.Include(p => p.User).ToList();
        }

        public IEnumerable<Basket> GetBaskets(int userId)
        {
            return DbSet.Where(p => p.UserId == userId).ToList();
        }

        public override Basket GetById(int id)
        {
            return DbSet.Include(p => p.User).FirstOrDefault(p => p.Id == id);
        }

        public Basket GetClose(int userId, int organizationId)
        {
            return DbSet.Include(p => p.User).FirstOrDefault(p => p.UserId == userId && 
                                                                  p.OrganizationId == organizationId &&
                                                                  p.Status == true);
        }

        public Basket GetOpen(int userId, int organizationId)
        {
            return DbSet.Include(p => p.User).FirstOrDefault(p => p.UserId == userId && 
                                                                  p.OrganizationId == organizationId &&
                                                                  p.Status == false);
        }
    }
}
