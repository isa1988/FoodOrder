using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.DAL.Unit.Contracts;
using FoodOrderWeb.Service.Dtos.Basket;
using FoodOrderWeb.Service.Dtos.BasketInventory;

namespace FoodOrderWeb.Service.Services.Contracts
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public BasketService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            if (unitOfWorkFactory == null)
                throw new ArgumentNullException(nameof(unitOfWorkFactory));

            _unitOfWorkFactory = unitOfWorkFactory;
        }


        public async Task<EntityOperationResult<Basket>> CreateOrEditItemAsync(BasketEditDto basketEditDto)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var basket = unitOfWork.Basket.GetOpen(basketEditDto.UserId, basketEditDto.OrganizationId);
                if (basket == null)
                {
                    var createDto = new BasketCreateDto
                    {
                        UserId = basketEditDto.UserId,
                        OrganizationId = basketEditDto.OrganizationId
                    };
                    foreach (var rec in basketEditDto.BasketInventoryDtos)
                    {
                        var basketInventory = new BasketInventoryCreateDto
                        {
                            DishId = rec.DishId,
                            CountInventory = rec.CountInventory,
                            Price = rec.Price
                        };
                        createDto.BasketInventoryDto.Add(basketInventory);
                    }

                    return await CreateItemAsync(createDto);
                }
                else
                {
                    return await EditItemAsync(basketEditDto);
                }
            }
        }

        public async Task<EntityOperationResult<Basket>> CreateItemAsync(BasketCreateDto basketCreateDto)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                int countInverory = basketCreateDto.BasketInventoryDto.Sum(x => x.CountInventory);
                
                if (countInverory <= 0)
                    return EntityOperationResult<Basket>
                        .Failure()
                        .AddError("Вы не указали количество товара");
                if (basketCreateDto.BasketInventoryDto.Any(x => x.CountInventory < 0))
                    return EntityOperationResult<Basket>
                        .Failure()
                        .AddError("Вы указали отрицательное число товара");

                try
                {
                    var basket = new Basket
                    {
                        UserId = basketCreateDto.UserId,
                        CountInventory = countInverory,
                        OrganizationId = basketCreateDto.OrganizationId,
                        Sum = basketCreateDto.BasketInventoryDto.Sum(x => x.Price * x.CountInventory),
                        Status = false,
                        DateAction = DateTime.Now
                    };

                    var entity = await unitOfWork.Basket.AddAsync(basket);
                    await unitOfWork.CompleteAsync();
                    var basketCheck = unitOfWork.Basket.GetById(entity.Id);
                    if (basketCheck != null)
                    {
                        foreach (var rec in basketCreateDto.BasketInventoryDto.Where(x => x.CountInventory > 0))
                        {
                            var basketInventory = new BasketInventory
                            {
                                BasketId = entity.Id,
                                DishId = rec.DishId,
                                CountInventory = rec.CountInventory,
                                Price = rec.Price,
                                Sum = rec.CountInventory * rec.Price
                            };
                            var basketInventoryCheck = await unitOfWork.BasketInventory.AddAsync(basketInventory);
                            await unitOfWork.CompleteAsync();
                        }
                    }

                    return EntityOperationResult<Basket>.Success(entity);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<Basket>.Failure().AddError(ex.Message);
                }
            }
        }

        public async Task<EntityOperationResult<Basket>> EditItemAsync(BasketEditDto basketEditDto)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                int countInverory = basketEditDto.BasketInventoryDtos.Sum(x => x.CountInventory);

                if (countInverory <= 0)
                    return EntityOperationResult<Basket>
                        .Failure()
                        .AddError("Вы не указали количество товара");
                if (basketEditDto.BasketInventoryDtos.Any(x => x.CountInventory < 0))
                    return EntityOperationResult<Basket>
                        .Failure()
                        .AddError("Вы указали отрицательное число товара");

                try
                {
                    var basket = unitOfWork.Basket.GetById(basketEditDto.Id);
                    basket.CountInventory = countInverory;
                    basket.Sum = basketEditDto.BasketInventoryDtos.Sum(x => x.Price * x.CountInventory);
                    basket.DateAction = DateTime.Now;

                    unitOfWork.Basket.Update(basket);
                    List<BasketInventory> basketInventories =
                        unitOfWork.BasketInventory.GetBasketInventories(basketEditDto.Id).ToList();
                    bool flagAdd = false;
                    foreach (var rec in basketEditDto.BasketInventoryDtos)
                    {
                        if (rec.CountInventory == 0)
                        {
                            var basketInventory = basketInventories.FirstOrDefault(x => x.Id == rec.Id);
                            if (basketInventory != null)
                            {
                                unitOfWork.BasketInventory.Delete(basketInventory);
                            }
                        }
                        else
                        {
                            var basketInventory = basketInventories.FirstOrDefault(x => x.Id == rec.Id);
                            flagAdd = false;
                            if (basketInventory == null)
                            {
                                basketInventory = new BasketInventory
                                {
                                    BasketId = basket.Id,
                                    
                                };
                                flagAdd = true;
                            }

                            basketInventory.DishId = rec.DishId;
                            basketInventory.CountInventory = rec.CountInventory;
                            basketInventory.Price = rec.Price;
                            basketInventory.Sum = rec.CountInventory * rec.Price;

                            if (flagAdd)
                            {
                                await unitOfWork.BasketInventory.AddAsync(basketInventory);
                            }
                            else
                            {
                                unitOfWork.BasketInventory.Update(basketInventory);
                            }
                        }
                    }
                    await unitOfWork.CompleteAsync();

                    return EntityOperationResult<Basket>.Success(basket);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<Basket>.Failure().AddError(ex.Message);
                }
            }
        }

        public async Task<EntityOperationResult<Basket>> DeleteItemAsync(BasketDeleteDto basketDeleteDto)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                try
                {
                    var basket = unitOfWork.Basket.GetById(basketDeleteDto.Id);
                    unitOfWork.Basket.Delete(basket);
                    
                    await unitOfWork.CompleteAsync();

                    return EntityOperationResult<Basket>.Success(basket);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<Basket>.Failure().AddError(ex.Message);
                }
            }
        }


        public async Task<EntityOperationResult<Basket>> PayItemAsync(BasketPayDto basketPayDto)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                try
                {
                    var basket = unitOfWork.Basket.GetById(basketPayDto.Id);
                    basket.DateAction = DateTime.Now;
                    basket.Status = true;
                    unitOfWork.Basket.Update(basket);

                    await unitOfWork.CompleteAsync();

                    return EntityOperationResult<Basket>.Success(basket);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<Basket>.Failure().AddError(ex.Message);
                }
            }
        }

        public async Task<EntityOperationResult<Basket>> RatingAndCommentAsync(BasketRatingAndCommentDto basketRatingAndCommentDto)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                try
                {
                    var basket = unitOfWork.Basket.GetById(basketRatingAndCommentDto.Id);
                    foreach (var rec in basketRatingAndCommentDto.BasketInventoryRatingAndCommentDtos)
                    {
                        var paitings = unitOfWork.Ratings.GetRating(basketRatingAndCommentDto.UserId, rec.Id);
                        if (paitings == null && rec.Rating > 0)
                        {
                            var dish = unitOfWork.Dish.GetById(rec.DishId);
                            dish.RatingTotal += rec.Rating;
                            dish.RatingCount += 1;
                            dish.RatingAverage =
                                (dish.RatingCount != 0) ? (int) (dish.RatingTotal / dish.RatingCount) : 0;
                            unitOfWork.Dish.Update(dish);
                            await unitOfWork.CompleteAsync();
                            var rating = new Rating();
                            rating.UserId = basketRatingAndCommentDto.UserId;
                            rating.BasketInventoryId = rec.Id;
                            rating.RatingNumber = rec.Rating;
                            await unitOfWork.Ratings.AddAsync(rating);
                            await unitOfWork.CompleteAsync();
                        }
                        else if (paitings != null && rec.Rating == 0)
                        {
                            var dish = unitOfWork.Dish.GetById(rec.DishId);
                            dish.RatingTotal -= paitings.RatingNumber;
                            dish.RatingCount -= 1;
                            dish.RatingAverage =
                                (dish.RatingCount != 0) ? (int)(dish.RatingTotal / dish.RatingCount) : 0;
                            unitOfWork.Dish.Update(dish);
                            await unitOfWork.CompleteAsync();
                            unitOfWork.Ratings.Delete(paitings);
                            await unitOfWork.CompleteAsync();
                        }
                        else if (paitings != null && rec.Rating > 0 && paitings.RatingNumber != rec.Rating)
                        {
                            var dish = unitOfWork.Dish.GetById(rec.DishId);
                            dish.RatingTotal -= paitings.RatingNumber;
                            dish.RatingTotal += rec.Rating;
                            dish.RatingAverage =
                                (dish.RatingCount != 0) ? (int)(dish.RatingTotal / dish.RatingCount) : 0;
                            unitOfWork.Dish.Update(dish);
                            await unitOfWork.CompleteAsync();
                            paitings.RatingNumber = rec.Rating;
                            unitOfWork.Ratings.Update(paitings);
                            await unitOfWork.CompleteAsync();
                        }

                        var comment = unitOfWork.Comments.GetComment(basketRatingAndCommentDto.UserId, rec.Id);
                        if (comment == null && rec.Comment?.Length > 0)
                        {
                            var commentCreate = new Comment
                            {
                                BasketInventoryId = rec.Id,
                                Text = rec.Comment,
                                UserId = basketRatingAndCommentDto.UserId
                            };
                            await unitOfWork.Comments.AddAsync(commentCreate);
                            await unitOfWork.CompleteAsync();
                        }
                        else if (comment != null && rec.Comment?.Length > 0)
                        {
                            comment.Text = rec.Comment;
                            unitOfWork.Comments.Update(comment);
                            await unitOfWork.CompleteAsync();
                        }
                        else if (comment != null && (rec.Comment == null || rec.Comment?.Length == 0))
                        {
                            comment.Text = rec.Comment;
                            unitOfWork.Comments.Delete(comment);
                            await unitOfWork.CompleteAsync();
                        }
                    }


                    return EntityOperationResult<Basket>.Success(basket);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<Basket>.Failure().AddError(ex.Message);
                }
            }
        }

    }
}
