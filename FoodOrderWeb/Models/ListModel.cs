using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderWeb.Core.DataBase;

namespace FoodOrderWeb.Models
{
    public class ListModel : PageInfoModel
    {
        public IEnumerable<Organization> Organizations { get; set; }
        public IEnumerable<FoodOrderWeb.Core.DataBase.Dish> Dishes { get; set; }
        public IEnumerable<FoodOrderWeb.Core.DataBase.Basket> Baskets{ get; set; }
        public int? OrganizationId { get; set; }

        public ListModel()
        {
            Organizations = new List<Organization>();
            Dishes = new List<FoodOrderWeb.Core.DataBase.Dish>();
            Baskets = new List<FoodOrderWeb.Core.DataBase.Basket>();
        }
    }
}
