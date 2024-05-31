using FoodOrderWeb.DAL.Repositories;
using FoodOrderWeb.Service.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDTO)
        {
            var response = await accountService.CreateAccount(userDTO);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDTO)
        {
            var response = await accountService.LoginAccount(loginDTO);
            return Ok(response);
        }
    }
}
