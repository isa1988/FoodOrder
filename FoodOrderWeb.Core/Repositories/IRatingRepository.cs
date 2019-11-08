using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderWeb.Core.DataBase;

namespace FoodOrderWeb.Core.Repositories
{
    public interface IRatingRepository : IRepository<Rating>
    {
        IEnumerable<Rating> GetAll(int userId, int basketInventoryId);
        Rating GetRating(int userId, int basketInventoryId);
        IEnumerable<Rating> GetUsers(int userId);
        IEnumerable<Rating> GetBasketInventories(int basketInventoryId);
        IEnumerable<Rating> GetDishes(int userId, int dishId);
    }
}
