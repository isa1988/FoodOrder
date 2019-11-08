using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.Core.Repositories;
using FoodOrderWeb.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodOrderWeb.DAL.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(DataDbContext context) : base(context)
        {
            DbSet = context.Comments;
        }

        public string GetComment(int dishId)
        {
            List<User> users = _context.Users.ToList();
            StringBuilder stringBuilder = new StringBuilder("");
            List<Comment> comments = GetDishes(dishId).ToList();
            if (comments?.Count > 0)
            {
                for (int i = 0; i < comments.Count; i++)
                {
                    stringBuilder.Append("Автор " + users.FirstOrDefault(x => x.Id == comments[i].UserId)?.UserName);
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.Append(comments[i].Text);
                    stringBuilder.Append(Environment.NewLine);
                }
            }
            return stringBuilder.ToString();
        }

        public Comment GetComment(int userId, int basketInventoryId)
        {
            return DbSet.Include(p => p.User).FirstOrDefault(p => p.UserId == userId && p.BasketInventoryId == basketInventoryId);
        }

        public IEnumerable<Comment> GetBasketInventories(int basketInventoryId)
        {
            return DbSet.Include(p => p.User).Where(p => p.BasketInventoryId == basketInventoryId).ToList();
        }

        public IEnumerable<Comment> GetDishes(int dishId)
        {
            List<BasketInventory> basketInventories = _context.BasketInventories.Where(x => x.DishId == dishId).ToList();
            if (basketInventories?.Count > 0)
            {
                return DbSet.Include(p => p.User).Where(p => basketInventories.Any(x => p.BasketInventoryId == x.Id)).ToList();
            }
            else
            {
                return new List<Comment>();
            }
        }

    }
}
