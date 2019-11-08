using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderWeb.Models
{
    public class OrganizationEditModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string PictureName { get; set; }
        public string PictureFormat { get; set; }
        public byte[] File { get; set; }
        public IFormFile WorkToFile { get; set; }
        public bool IsPictureDelete { get; set; }
        
        public string Comment { get; set; }
    }
}
