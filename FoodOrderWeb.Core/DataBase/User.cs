using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace FoodOrderWeb.Core.DataBase
{
    public class User : IdentityUser<int>
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String PatronymicName { get; set; }
        public String FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Sex { get; set; }
        public bool IsBloked { get; set; }
        public ICollection<Basket> Baskets { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}