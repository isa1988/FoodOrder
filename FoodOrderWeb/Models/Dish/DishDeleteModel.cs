using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderWeb.Models.Dish
{
    public class DishDeleteModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Наименование не должно быть пустым")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int OrganizationId { get; set; }
        public string Organization { get; set; }
        public string PictureName { get; set; }
        public string PictureFormat { get; set; }
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}
