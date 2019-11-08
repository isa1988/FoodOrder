using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.Service.Dtos.Organization;
using FoodOrderWeb.Service.Dtos.Dish;

namespace FoodOrderWeb.Service
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            PointOfSaleMapping();
        }

        private void PointOfSaleMapping()
        {
            CreateMap<OrganizationCreateDto, Organization>();
            CreateMap<OrganizationEditDto, Organization>();
            CreateMap<DishCreateDto, Dish>();
            CreateMap<DishEditDto, Dish>();
        }
    }
}
