using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderWeb.Service.Dtos.BasketInventory;

namespace FoodOrderWeb.Service.Dtos.Basket
{
    public class BasketCreateDto
    {
        public int UserId { get; set; }
        public List<BasketInventoryCreateDto> BasketInventoryDto { get; set; }
        public decimal Sum { get; set; }
        public int OrganizationId { get; set; }

        public BasketCreateDto()
        {
            BasketInventoryDto = new List<BasketInventoryCreateDto>();
        }
    }
}
