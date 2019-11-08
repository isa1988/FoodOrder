using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.Service.Dtos.Organization;

namespace FoodOrderWeb.Service.Services.Contracts
{
    public interface IOrganizationService : IServiceWorkFile<Organization, OrganizationCreateDto, OrganizationEditDto, OrganizationDeleteDto>
    {
    }
}
