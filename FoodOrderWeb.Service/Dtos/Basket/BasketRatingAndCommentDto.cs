using FoodOrderWeb.Service.Dtos.BasketInventory;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.Service.Dtos.Basket
{
    public class BasketRatingAndCommentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<BasketInventoryRatingAndCommentDto> BasketInventoryRatingAndCommentDtos { get; set; }

        public BasketRatingAndCommentDto()
        {
            BasketInventoryRatingAndCommentDtos = new List<BasketInventoryRatingAndCommentDto>();
        }
    }
}
