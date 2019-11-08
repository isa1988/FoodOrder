using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.DAL.Unit.Contracts;
using FoodOrderWeb.Service.Dtos.Organization;
using FoodOrderWeb.Service.Services.Contracts;

namespace FoodOrderWeb.Service.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public OrganizationService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            if (unitOfWorkFactory == null)
                throw new ArgumentNullException(nameof(unitOfWorkFactory));

            _unitOfWorkFactory = unitOfWorkFactory;
        }
        
        public async Task<EntityOperationResult<Organization>> CreateItemAsync(OrganizationCreateDto createDto, string path)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (unitOfWork.Organization.IsExistByNAme(createDto.Name))
                    return EntityOperationResult<Organization>
                        .Failure()
                        .AddError($"Кафе с именем {createDto.Name} уже существует");

                try
                {
                    var organization = new Organization
                    {
                        Name = createDto.Name,
                        PictureName = System.Guid.NewGuid().ToString(),
                        Comment = createDto.Comment,
                    };

                    var fileService = new FileService(createDto.File, path + "\\" + 
                                                              organization.PictureName, createDto.PictureName);
                    organization.PictureFormat = fileService.GetTypeFile();
                    var entity = await unitOfWork.Organization.AddAsync(organization);
                    await unitOfWork.CompleteAsync();

                    fileService.Add();
                    return EntityOperationResult<Organization>.Success(organization);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<Organization>.Failure().AddError(ex.Message);
                }
            }
        }
        
        public async Task<EntityOperationResult<Organization>> EditItemAsync(OrganizationEditDto editDto, string path)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (unitOfWork.Organization.IsExistByNAme(editDto.Id, editDto.Name))
                    return EntityOperationResult<Organization>
                        .Failure()
                        .AddError($"Кафе с именем {editDto.Name} уже существует");

                try
                {
                    var organization = unitOfWork.Organization.GetById(editDto.Id);
                    string oldPictureFormat = organization.PictureFormat;
                    var fileService = new FileService(editDto.File, path + "\\" + organization.PictureName, 
                                                              editDto.PictureName, organization.PictureFormat);
                    organization.Name = editDto.Name;
                    if (editDto.File?.Length > 0 && !editDto.IsPictureDelete)
                    {
                        organization.PictureFormat = fileService.GetTypeFile();
                    }
                    else if (editDto.IsPictureDelete)
                    {
                        organization.PictureFormat = string.Empty;
                    }
                    organization.Comment = editDto.Comment;

                    unitOfWork.Organization.Update(organization);
                    await unitOfWork.CompleteAsync();

                    fileService.AddOrDelete(editDto.IsPictureDelete);

                    return EntityOperationResult<Organization>.Success(organization);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<Organization>.Failure().AddError(ex.Message);
                }
            }
        }

        public async Task<EntityOperationResult<Organization>> DeleeteItemAsync(OrganizationDeleteDto deleteDto, string path)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (unitOfWork.Organization.IsHasDishesh(deleteDto.Id))
                    return EntityOperationResult<Organization>
                        .Failure()
                        .AddError($"Кафе с именем {deleteDto.Name} имеет блюда");
                try
                {
                    var organization = unitOfWork.Organization.GetById(deleteDto.Id);
                    FileService fileService = new FileService(null, path + "/Org/\\" + organization.PictureName,
                                                              string.Empty, organization.PictureFormat);
                    fileService.AddOrDelete(true);
                    unitOfWork.Organization.Delete(organization);
                    await unitOfWork.CompleteAsync();

                    return EntityOperationResult<Organization>.Success(organization);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<Organization>.Failure().AddError(ex.Message);
                }
            }
        }

    }
}
