using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodOrderWeb.Service.Dtos.Dish
{
    public class DishDeleteDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        
        public string PictureName { get; set; }
        public string PictureFormat { get; set; }

        public string Comment { get; set; }
    }
}
