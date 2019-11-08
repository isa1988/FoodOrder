using FoodOrderWeb.DAL.Data.Contracts;
using FoodOrderWeb.DAL.Unit.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.DAL.Unit
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IDataDbContextFactory _applicationDbContextFactory;

        public UnitOfWorkFactory(IDataDbContextFactory applicationDbContextFactory)
        {
            _applicationDbContextFactory = applicationDbContextFactory;
        }

        public IUnitOfWork MakeUnitOfWork()
        {
            return new UnitOfWork(_applicationDbContextFactory.Create());
        }
    }
}
