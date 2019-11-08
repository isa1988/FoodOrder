using FoodOrderWeb.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderWeb.Service.Services.Contracts
{
    public interface IServiceWorkFile<T, I, U, D> where T : Entity
    {
        Task<EntityOperationResult<T>> CreateItemAsync(I createDto, string path);
        Task<EntityOperationResult<T>> EditItemAsync(U editDto, string path);
        Task<EntityOperationResult<T>> DeleeteItemAsync(D deleteDto, string path);
    }
}
