using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderWeb.Core.DataBase;

namespace FoodOrderWeb.Core.Repositories
{
    public interface IDishRepository : IRepository<Dish>
    {
        bool IsExistByNAme(string name, int organizationId);
        bool IsExistByNAme(int id, string name, int organizationId);
        IEnumerable<Dish> GetAll(int organizationId);
    }
}
