using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.Core.DataBase
{
    public class Rating : Entity
    {
        public int BasketInventoryId { get; set; }
        public int UserId { get; set; }
        public int RatingNumber { get; set; }
        public virtual BasketInventory BasketInventory { get; set; }
        public virtual User User { get; set; }
    }
}
