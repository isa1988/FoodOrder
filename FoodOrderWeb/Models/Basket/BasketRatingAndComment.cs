using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderWeb.Models.BaketInventory;

namespace FoodOrderWeb.Models.Basket
{
    public class BasketRatingAndComment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrganizationId { get; set; }

        public List<BasketInventoryRatingAndComment> BasketInventoryRatingAndComments { get; set; }

        public BasketRatingAndComment()
        {
            BasketInventoryRatingAndComments = new List<BasketInventoryRatingAndComment>();
        }
    }
}
