using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.Service.Dtos.Dish;

namespace FoodOrderWeb.Service.Services.Contracts
{
    public interface IDishService : IServiceWorkFile<Dish, DishCreateDto, DishEditDto, DishDeleteDto>
    {
        
    }

}
