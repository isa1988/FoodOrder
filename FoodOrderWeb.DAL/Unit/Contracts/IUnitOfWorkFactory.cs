using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.DAL.Unit.Contracts
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork MakeUnitOfWork();
    }
}
