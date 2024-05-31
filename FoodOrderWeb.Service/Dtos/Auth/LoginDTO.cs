using System.ComponentModel.DataAnnotations;

namespace FoodOrderWeb.Service.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
