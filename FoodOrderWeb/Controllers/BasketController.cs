using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.DAL.Unit.Contracts;
using FoodOrderWeb.Models;
using FoodOrderWeb.Models.Basket;
using FoodOrderWeb.Service.Dtos.Basket;
using FoodOrderWeb.Service.Dtos.BasketInventory;
using FoodOrderWeb.Service.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly IBasketService _service;

        public BasketController(
            IUnitOfWorkFactory unitOfWorkFactory, 
            UserManager<User> userManager,
            IBasketService service)
            : base(unitOfWorkFactory)
        {
            if (userManager == null)
                throw new ArgumentNullException(nameof(userManager));
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            _userManager = userManager;
            _service = service;
        }

        [HttpGet("Get")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetBasketForUser()
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var userId = _userManager.GetUserId(User);
                var baskets = unitOfWork.Basket.GetBaskets(int.Parse(userId));
                var model = new ListModel
                {
                    Baskets = baskets
                };
                return Ok(model);
            }
        }

        [HttpPost("CreateOrEdit")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateOrEditBasket(BasketRatingAndComment request)
        {
            var basketEditDto = new BasketEditDto
            {
                Id = request.Id,
                UserId = request.UserId,
                OrganizationId = request.OrganizationId
            };

            foreach (var rec in request.BasketInventoryRatingAndComments)
            {
                var basketInventoryEditDto = new BasketInventoryEditDto
                {
                    Id = rec.Id,
                    CountInventory = rec.CountInventory,
                    Price = rec.Price,
                    DishId = rec.DishId,
                    Sum = rec.Sum
                };
                basketEditDto.BasketInventoryDtos.Add(basketInventoryEditDto);
            }

            var result = await _service.CreateOrEditItemAsync(basketEditDto);

            return Ok(result);
        }

        [HttpPost("Pay")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PayForBasket(BasketEditModel request)
        {
            var basketPayDto = new BasketPayDto
            {
                Id = request.Id,
                Sum = request.Sum
            };

            var result = await _service.PayItemAsync(basketPayDto);

            return Ok(result);
        }

        [HttpPost("RatingAndComment")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RateAndCommentBasket(BasketRatingAndComment request)
        {
            var model = new BasketRatingAndCommentDto()
            {
                Id = request.Id,
                UserId = request.UserId
            };

            foreach (var rec in request.BasketInventoryRatingAndComments)
            {
                var basketInventoryRatingAndComment = new BasketInventoryRatingAndCommentDto()
                {
                    Id = rec.Id,
                    DishId = rec.DishId,
                    Comment = rec.Comment,
                    Rating = rec.Raiting
                };
                model.BasketInventoryRatingAndCommentDtos.Add(basketInventoryRatingAndComment);
            }

            var result = await _service.RatingAndCommentAsync(model);

            return Ok(result);
        }

        [HttpPost("Delete")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteBasket(BasketEditModel request)
        {
            var basketDeleteDto = new BasketDeleteDto
            {
                Id = request.Id,
            };

            var result = await _service.DeleteItemAsync(basketDeleteDto);

            return Ok(result);
        }
    }
}