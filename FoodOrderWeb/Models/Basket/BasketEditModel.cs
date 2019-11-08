using FoodOrderWeb.Models.BaketInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderWeb.Models.Basket
{
    public class BasketEditModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Sum { get; set; }
        public int OrganizationId { get; set; }

        public List<BasketInventoryEditModel> BasketInventoryEditModels { get; set; }

        public BasketEditModel()
        {
            BasketInventoryEditModels = new List<BasketInventoryEditModel>();
        }
    }
}
