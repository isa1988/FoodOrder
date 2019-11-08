using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodOrderWeb.Service.Dtos.Organization
{
    public class OrganizationEditDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string PictureName { get; set; }
        public string PictureFormat { get; set; }
        public byte[] File { get; set; }
        public bool IsPictureDelete { get; set; }
        public string Comment { get; set; }
    }
}
