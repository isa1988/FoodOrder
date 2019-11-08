using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.Service.Dtos.Basket;

namespace FoodOrderWeb.Service.Services.Contracts
{
    public interface IBasketService
    {
        Task<EntityOperationResult<Basket>> CreateItemAsync(BasketCreateDto basketCreateDto);
        Task<EntityOperationResult<Basket>> EditItemAsync(BasketEditDto basketEditDto);
        Task<EntityOperationResult<Basket>> CreateOrEditItemAsync(BasketEditDto basketEditDto);
        Task<EntityOperationResult<Basket>> DeleteItemAsync(BasketDeleteDto basketDeleteDto);
        Task<EntityOperationResult<Basket>> PayItemAsync(BasketPayDto basketPayDto);
        Task<EntityOperationResult<Basket>> RatingAndCommentAsync(BasketRatingAndCommentDto basketRatingAndCommentDto);
    }
}
