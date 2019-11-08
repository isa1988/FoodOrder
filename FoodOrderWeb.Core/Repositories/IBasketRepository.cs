using FoodOrderWeb.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderWeb.Core.Repositories
{
    public interface IBasketRepository : IRepository<Basket>
    {
        IEnumerable<Basket> GetBaskets(int userId);
        Basket GetOpen(int userId, int organizationId);
        Basket GetClose(int userId, int organizationId);
    }
}
