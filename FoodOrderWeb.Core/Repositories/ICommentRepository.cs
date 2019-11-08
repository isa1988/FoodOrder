using FoodOrderWeb.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.Core.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IEnumerable<Comment> GetBasketInventories(int basketInventoryId);
        IEnumerable<Comment> GetDishes(int dishId);
        string GetComment(int dishId);
        Comment GetComment(int userId, int basketInventoryId);
    }
}
