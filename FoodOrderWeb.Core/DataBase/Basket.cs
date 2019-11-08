using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.Core.DataBase
{
    public class Basket : Entity
    {
        public DateTime DateAction { get; set; }
        public int UserId { get; set; }
        public int CountInventory { get; set; }
        public int OrganizationId { get; set; }
        public decimal Sum { get; set; }
        public ICollection<BasketInventory> BasketInventories { get; set; }
        public bool Status { get; set; }
        public virtual User User { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
