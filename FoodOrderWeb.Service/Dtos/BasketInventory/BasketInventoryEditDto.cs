﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.Service.Dtos.BasketInventory
{
    public class BasketInventoryEditDto
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int CountInventory { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get; set; }
    }
}
