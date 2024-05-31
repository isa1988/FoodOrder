using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.DAL.Data;
using FoodOrderWeb.DAL.Unit.Contracts;
using FoodOrderWeb.Service.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderWeb.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public BaseController(
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            if (unitOfWorkFactory == null)
                throw new ArgumentNullException(nameof(unitOfWorkFactory));

            _unitOfWorkFactory = unitOfWorkFactory;
        }
    }
}