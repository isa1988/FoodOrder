using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.DAL.Unit.Contracts;
using FoodOrderWeb.Service.Dtos.Basket;
using FoodOrderWeb.Service.Dtos.BasketInventory;
using FoodOrderWeb.Service.Services.Contracts;
using FoodOrderWeb.Models;
using FoodOrderWeb.Models.BaketInventory;
using FoodOrderWeb.Models.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderWeb.Controllers
{
    [Authorize(Roles = "User")]
    public class BasketController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly IBasketService _service;

        public BasketController(IUnitOfWorkFactory unitOfWorkFactory, UserManager<User> userManager,
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

        public IActionResult Index()
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var userId = _userManager.GetUserId(User);
                var baskets = unitOfWork.Basket.GetBaskets(int.Parse(userId));
                var model = new ListModel
                {
                    Baskets = baskets
                };
                return View(model);
            }
        }

        public IActionResult CreateOrEdit(int? organizationId)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (!organizationId.HasValue)
                {
                    return NotFound();
                }

                var userId = _userManager.GetUserId(User);
                /*var basketCheck = unitOfWork.Basket.GetClose(int.Parse(userId), organizationId.Value);
                if (basketCheck != null)
                {
                    return NotFound();
                }*/

                var basket = unitOfWork.Basket.GetOpen(int.Parse(userId), organizationId.Value);
                var dishes = unitOfWork.Dish.GetAll(organizationId.Value);
                var basketInventories = new List<BasketInventory>();
                if (basket != null)
                {
                    basketInventories = unitOfWork.BasketInventory.GetBasketInventories(basket.Id).ToList();
                }

                var model = new BasketRatingAndComment
                {
                    Id = basket?.Id ?? 0,
                    UserId = int.Parse(userId),
                    OrganizationId = organizationId.Value,
                };

                foreach (var dish in dishes)
                {
                    var comment = unitOfWork.Comments.GetComment(dish.Id);
                    var basketInventory = new BasketInventoryRatingAndComment
                    {
                        DishId = dish.Id,
                        DishComment = dish.Comment,
                        Name = dish.Name,
                        PictureFormat = dish.PictureFormat,
                        PictureName = dish.PictureName,
                        Price = dish.Price,
                        Comment = comment,
                        Raiting = dish.RatingAverage,
                        RatingCount = dish.RatingCount
                    };
                    var tempBasketInventory = basketInventories.FirstOrDefault(x => x.DishId == dish.Id);
                    if (tempBasketInventory != null)
                    {
                        basketInventory.Id = tempBasketInventory.Id;
                        basketInventory.CountInventory = tempBasketInventory.CountInventory;
                        basketInventory.Sum = tempBasketInventory.Sum;
                    }

                    model.BasketInventoryRatingAndComments.Add(basketInventory);
                }

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(BasketRatingAndComment request)
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

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var resultError in result.Errors)
                {
                    ModelState.AddModelError("Error", resultError);
                }

                return View(request);
            }
        }
        
        public IActionResult Pay(int? id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (!id.HasValue)
                {
                    return NotFound();
                }

                var basket = unitOfWork.Basket.GetById(id.Value);
                if (basket == null || basket.Status)
                {
                    return NotFound();
                }

                var basketInventories = unitOfWork.BasketInventory.GetBasketInventories(basket.Id).ToList();
                
                var model = new BasketEditModel
                {
                    Id = basket.Id,
                    UserId = basket.UserId,
                    OrganizationId = basket.OrganizationId,
                    Sum = basket.Sum
                };

                foreach (var rec in basketInventories)
                {
                    var basketInventory = new BasketInventoryEditModel
                    {
                        Id = rec.Id,
                        Name = rec.Dish?.Name ?? string.Empty,
                        CountInventory = rec.CountInventory,
                        Sum = rec.Sum,
                        DishId = rec.DishId,
                        Comment = rec.Dish?.Comment ?? string.Empty,
                        PictureFormat = rec.Dish?.PictureFormat ?? string.Empty,
                        PictureName = rec.Dish?.PictureName ?? string.Empty,
                        Price = rec.Price,
                    };
                    model.BasketInventoryEditModels.Add(basketInventory);
                }

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(BasketEditModel request)
        {
            var basketPayDto = new BasketPayDto
            {
                Id = request.Id,
                Sum = request.Sum
            };

            var result = await _service.PayItemAsync(basketPayDto);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var resultError in result.Errors)
                {
                    ModelState.AddModelError("Error", resultError);
                }

                return View(request);
            }
        }

        public IActionResult Delete(int? id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (!id.HasValue)
                {
                    return NotFound();
                }
                
                var basket = unitOfWork.Basket.GetById(id.Value);
                if (basket == null)
                {
                    return NotFound();
                }
                var basketInventories = unitOfWork.BasketInventory.GetBasketInventories(basket.Id).ToList();
                
                var model = new BasketEditModel
                {
                    Id = basket?.Id ?? 0,
                    UserId = basket.UserId,
                    OrganizationId = basket.OrganizationId,
                    Sum = basket.Sum
                };

                foreach (var rec in basketInventories)
                {
                    var basketInventory = new BasketInventoryEditModel
                    {
                        Id = rec.Id,
                        CountInventory = rec.CountInventory,
                        Sum = rec.Sum,
                        DishId = rec.DishId,
                        Comment = rec.Dish?.Comment ?? string.Empty,
                        PictureFormat = rec.Dish?.PictureFormat ?? string.Empty,
                        PictureName = rec.Dish?.PictureName ?? string.Empty,
                        Price = rec.Price,
                    };

                    model.BasketInventoryEditModels.Add(basketInventory);
                }

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(BasketEditModel request)
        {
            var basketDeleteDto = new BasketDeleteDto
            {
                Id = request.Id,
            };

            var result = await _service.DeleteItemAsync(basketDeleteDto);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var resultError in result.Errors)
                {
                    ModelState.AddModelError("Error", resultError);
                }

                return View(request);
            }
        }

        public IActionResult RatingAndComment(int? id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (!id.HasValue)
                {
                    return NotFound();
                }

                var basket = unitOfWork.Basket.GetById(id.Value);
                if (basket == null)
                {
                    return NotFound();
                }
                var basketInventories = unitOfWork.BasketInventory.GetBasketInventories(basket.Id).ToList();
                var userId = _userManager.GetUserId(User);
                var model = new BasketRatingAndComment
                {
                    Id = basket?.Id ?? 0,
                    UserId = int.Parse(userId),
                    OrganizationId = basket.OrganizationId
                };

                foreach (var rec in basketInventories)
                {
                    var rating = unitOfWork.Ratings.GetRating(int.Parse(userId), rec.Id);
                    var comment = unitOfWork.Comments.GetComment(int.Parse(userId), rec.Id);
                    var basketInventory = new BasketInventoryRatingAndComment
                    {
                        Id = rec.Id,
                        CountInventory = rec.CountInventory,
                        Sum = rec.Sum,
                        DishId = rec.DishId,
                        Name = rec.Dish?.Name ?? string.Empty,
                        Comment = comment?.Text ?? string.Empty,
                        PictureFormat = rec.Dish?.PictureFormat ?? string.Empty,
                        PictureName = rec.Dish?.PictureName ?? string.Empty,
                        Price = rec.Price,
                        Raiting = rating?.RatingNumber ?? 0
                    };

                    model.BasketInventoryRatingAndComments.Add(basketInventory);
                }

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RatingAndComment(BasketRatingAndComment request)
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

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var resultError in result.Errors)
                {
                    ModelState.AddModelError("Error", resultError);
                }

                return View(request);
            }
        }
    }
}