using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.Core.DataBase
{
    public class Dish : Entity
    {
        public string Name { get; set; }

        public decimal Price { get; set; }


        public string PictureName { get; set; }
        public string PictureFormat { get; set; }

        public string Comment { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public int RatingTotal { get; set; }
        public int RatingAverage { get; set; }
        public int RatingCount { get; set; }

    }
}
