using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.Core.Repositories;
using FoodOrderWeb.DAL.Data;

namespace FoodOrderWeb.DAL.Repositories
{
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        public RatingRepository(DataDbContext context) : base(context)
        {
            DbSet = context.Ratings;
        }

        public IEnumerable<Rating> GetAll(int userId, int basketInventoryId)
        {
            return DbSet.Where(x => x.UserId == userId && x.BasketInventoryId == basketInventoryId);
        }

        public Rating GetRating(int userId, int basketInventoryId)
        {
            return DbSet.FirstOrDefault(x => x.UserId == userId && x.BasketInventoryId == basketInventoryId);
        }

        public IEnumerable<Rating> GetBasketInventories(int basketInventoryId)
        {
            return DbSet.Where(x => x.BasketInventoryId == basketInventoryId);
        }

        public IEnumerable<Rating> GetUsers(int userId)
        {
            return DbSet.Where(x => x.UserId == userId);
        }

        public IEnumerable<Rating> GetDishes(int userId, int dishId)
        {
            List<BasketInventory> basketInventories = _context.BasketInventories.Where(x => x.BasketId == dishId).ToList();
            if (basketInventories?.Count > 0)
            {
                return DbSet.Where(x => x.UserId == userId && basketInventories.Any(p => x.BasketInventoryId == p.Id));
            }
            else
            {
                return new List<Rating>();
            }
        }
    }
}
