using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.DAL.Data.Contracts
{
    public interface IDataDbContextFactory
    {
        DataDbContext Create();
    }
}
