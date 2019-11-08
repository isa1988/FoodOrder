using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderWeb.Service.Dtos.Organization;
using FoodOrderWeb.Models;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.Models.Dish;
using FoodOrderWeb.Service.Dtos.Dish;

namespace FoodOrderWeb
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            OrganizationMapping();
            DishMapping();
        }

        private void OrganizationMapping()
        {
            CreateMap<OrganizationCreatingModel, OrganizationCreateDto>();
            CreateMap<OrganizationCreateDto, OrganizationCreatingModel>();

            CreateMap<OrganizationEditModel, OrganizationEditDto>();
            CreateMap<OrganizationEditDto, OrganizationEditModel>();
            CreateMap<Organization, OrganizationEditModel>();

            CreateMap<OrganizationDeleteModel, OrganizationDeleteDto>();
            CreateMap<OrganizationDeleteDto, OrganizationDeleteModel>();
            CreateMap<Organization, OrganizationDeleteModel>();
        }

        private void DishMapping()
        {
            CreateMap<DishCreatingModel, DishCreateDto>();
            CreateMap<DishCreateDto, DishCreatingModel>();

            CreateMap<DishEditModel, DishEditDto>();
            CreateMap<DishEditDto, DishEditModel>();
            CreateMap<Dish, DishEditModel>();

            CreateMap<DishDeleteModel, DishDeleteDto>();
            CreateMap<DishDeleteDto, DishDeleteModel>();
            CreateMap<Dish, DishDeleteModel>();
        }
    }
}
