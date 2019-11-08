using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FoodOrderWeb.Models
{
    public class OrganizationCreatingModel : PageInfoModel
    {
        public string Name { get; set; }

        public string PictureName { get; set; }
        public string PictureFormat { get; set; }
        public byte[] File { get; set; }
        public IFormFile WorkToFile { get; set; }

        public string Comment { get; set; }
    }
}
