using FoodOrderWeb.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderWeb.Core.Repositories
{
    public interface IBasketInventoryRepository : IRepository<BasketInventory>
    {
        IEnumerable<BasketInventory> GetBasketInventories(int basketId);
    }
}
