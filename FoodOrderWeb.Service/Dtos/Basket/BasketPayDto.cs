using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.Service.Dtos.Basket
{
    public class BasketPayDto
    {
        public int Id { get; set; }
        public int CountInventory { get; set; }
        public decimal Sum { get; set; }
        public bool Status { get; set; }
    }
}
