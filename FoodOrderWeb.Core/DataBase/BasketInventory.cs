using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.Core.DataBase
{
    public class BasketInventory : Entity
    {
        public int DishId { get; set; }
        public int BasketId { get; set; }
        public int CountInventory { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get; set; }
        public Basket Basket { get; set; }
        public Dish Dish { get; set; }

        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
