using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.Core.DataBase
{
    public class Organization : Entity
    {
        public string Name { get; set; }

        public string PictureName { get; set; }
        public string PictureFormat { get; set; }

        public string Comment { get; set; }

        public ICollection<Dish> Dishes { get; set; }
        public ICollection<Basket> Baskets { get; set; }

        public Organization()
        {
            Dishes = new List<Dish>();
            Baskets = new List<Basket>();
        }
    }
}
