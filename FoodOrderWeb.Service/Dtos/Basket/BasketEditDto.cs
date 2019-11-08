using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderWeb.Service.Dtos.BasketInventory;

namespace FoodOrderWeb.Service.Dtos.Basket
{
    public class BasketEditDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public List<BasketInventoryEditDto> BasketInventoryDtos { get; set; }
        public decimal Sum { get; set; }

        public BasketEditDto()
        {
            BasketInventoryDtos = new List<BasketInventoryEditDto>();
        }
    }
}
