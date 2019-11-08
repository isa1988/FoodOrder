using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.Service.Dtos.BasketInventory
{
    public class BasketInventoryRatingAndCommentDto
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
