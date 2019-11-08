using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderWeb.Models.Dish
{
    public class DishEditModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Наименование не должно быть пустым")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Organization")]
        public int OrganizationId { get; set; }

        public decimal Price { get; set; }

        public SelectList OrganizationList { get; set; }
        public string PictureName { get; set; }
        public string PictureFormat { get; set; }
        public byte[] File { get; set; }
        public IFormFile WorkToFile { get; set; }
        public bool IsPictureDelete { get; set; }
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}
