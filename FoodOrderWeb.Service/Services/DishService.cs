using AutoMapper;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.DAL.Unit.Contracts;
using FoodOrderWeb.Service.Dtos.Dish;
using FoodOrderWeb.Service.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderWeb.Service.Services
{
    public class DishService : IDishService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public DishService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            if (unitOfWorkFactory == null)
                throw new ArgumentNullException(nameof(unitOfWorkFactory));

            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<EntityOperationResult<Dish>> CreateItemAsync(DishCreateDto createDto, string path)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (unitOfWork.Dish.IsExistByNAme(createDto.Name, createDto.OrganizationId))
                    return EntityOperationResult<Dish>
                        .Failure()
                        .AddError($"Блюдо с именем {createDto.Name} уже существует");
                if (createDto.Price <= 0)
                    return EntityOperationResult<Dish>
                        .Failure()
                        .AddError("Цена не может быть меньше или равняться нулю");
                try
                {
                    var dish = new Dish
                    {
                        Name = createDto.Name,
                        OrganizationId = createDto.OrganizationId,
                        Price = createDto.Price,
                        PictureName = System.Guid.NewGuid().ToString(),
                        Comment = createDto.Comment
                    };

                    var fileService = new FileService(createDto.File, path + "\\" +
                                                     dish.PictureName, createDto.PictureName);
                    dish.PictureFormat = fileService.GetTypeFile();
                    var entity = await unitOfWork.Dish.AddAsync(dish);
                    await unitOfWork.CompleteAsync();

                    fileService.Add();

                    return EntityOperationResult<Dish>.Success(entity);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<Dish>.Failure().AddError(ex.Message);
                }
            }
        }

        public async Task<EntityOperationResult<Dish>> EditItemAsync(DishEditDto editDto, string path)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (unitOfWork.Dish.IsExistByNAme(editDto.Id, editDto.Name, editDto.OrganizationId))
                    return EntityOperationResult<Dish>
                        .Failure()
                        .AddError($"Блюдо с именем {editDto.Name} уже существует");
                if (editDto.Price <= 0)
                    return EntityOperationResult<Dish>
                        .Failure()
                        .AddError("Цена не может быть меньше или равняться нулю");

                try
                {
                    var dish = unitOfWork.Dish.GetById(editDto.Id);
                    string oldPictureFormat = dish.PictureFormat;
                    var fileService = new FileService(editDto.File, path + "\\" + dish.PictureName,
                        editDto.PictureName, dish.PictureFormat);
                    dish.Name = editDto.Name;
                    dish.Price = editDto.Price;
                    if (editDto.File?.Length > 0 && !editDto.IsPictureDelete)
                    {
                        dish.PictureFormat = fileService.GetTypeFile();
                    }
                    else if (editDto.IsPictureDelete)
                    {
                        dish.PictureFormat = string.Empty;
                    }
                    dish.Comment = editDto.Comment;

                    unitOfWork.Dish.Update(dish);
                    await unitOfWork.CompleteAsync();

                    fileService.AddOrDelete(editDto.IsPictureDelete);

                    return EntityOperationResult<Dish>.Success(dish);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<Dish>.Failure().AddError(ex.Message);
                }
            }
        }

        public async Task<EntityOperationResult<Dish>> DeleteItemAsync(DishDeleteDto deleteDto, string path)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                try
                {
                    var dish = unitOfWork.Dish.GetById(deleteDto.Id);
                    unitOfWork.Dish.Delete(dish);
                    await unitOfWork.CompleteAsync();

                    if (dish.PictureName?.Length > 0)
                    {
                        FileService fileService = new FileService(null, path + "\\" + dish.PictureName,
                            string.Empty, dish.PictureFormat);
                        fileService.AddOrDelete(true);
                    }

                    return EntityOperationResult<Dish>.Success(dish);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<Dish>.Failure().AddError(ex.Message);
                }
            }
        }

    }
}
