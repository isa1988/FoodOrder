using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderWeb.Models.BaketInventory
{
    public class BasketInventoryRatingAndComment
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int CountInventory { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get; set; }
        public string Name { get; set; }
        public string PictureName { get; set; }
        public string PictureFormat { get; set; }
        public string DishComment { get; set; }
        public int RatingCount { get; set; }
        public string Comment { get; set; }
        public int Raiting { get; set; }
    }
}
