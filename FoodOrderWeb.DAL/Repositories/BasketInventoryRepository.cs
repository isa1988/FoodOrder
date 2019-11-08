using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.Core.Repositories;
using FoodOrderWeb.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderWeb.DAL.Repositories
{
    class BasketInventoryRepository : Repository<BasketInventory>, IBasketInventoryRepository
    {
        public BasketInventoryRepository(DataDbContext context) : base(context)
        {
            DbSet = context.BasketInventories;
        }

        public override IEnumerable<BasketInventory> GetAll()
        {
            return DbSet.Include(p => p.Basket)
                        .Include(p => p.Dish).ToList();
        }

        public override BasketInventory GetById(int id)
        {
            return DbSet.Include(p => p.Basket)
                        .Include(p => p.Dish)
                        .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<BasketInventory> GetBasketInventories(int basketId)
        {
            return DbSet.Include(p => p.Basket)
                        .Include(p => p.Dish)
                        .Where(p => p.BasketId == basketId).ToList();
        }
    }
}
