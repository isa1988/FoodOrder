using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderWeb.Core.DataBase;

namespace FoodOrderWeb.Core.Repositories
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        bool IsExistByNAme(string name);
        bool IsExistByNAme(int id, string name);
        bool IsHasDishesh(int id);
    }
}
