using FoodOrderWeb.DAL.Unit.Contracts;
using FoodOrderWeb.Models;
using FoodOrderWeb.Service.Dtos.Organization;
using FoodOrderWeb.Service.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : BaseController
    {
        private readonly IOrganizationService _service;

        public OrganizationController(
            IUnitOfWorkFactory unitOfWorkFactory, 
            IOrganizationService organizationService) : 
            base(unitOfWorkFactory)
        {
            this._service = organizationService;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetOrganizations()
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var organizations = unitOfWork.Organization.GetAll();
                var model = new ListModel
                {
                    Organizations = organizations
                };
                return Ok(model);
            }
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrganization(
            OrganizationCreatingModel request)
        {
            var dto = new OrganizationCreateDto()
            {
                Name = request.Name,
                PictureName = request.PictureName,
                PictureFormat = request.PictureFormat,
                Comment = request.Comment,
                File = request.File,
            };

            // путь к папке Files
            string path = string.Empty;

            if (request.WorkToFile != null)
            {
                byte[] imageData = [];
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(request.WorkToFile.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)request.WorkToFile.Length);
                }

                dto.File = imageData;
                dto.PictureName = request.WorkToFile.FileName;
                path = ""; //_appEnvironment.WebRootPath + "/Images/Org/";
            }

            var result = await _service.CreateItemAsync(dto, path);
            return Ok(result);
        }

        [HttpPost("Edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditOrganization(
            OrganizationEditModel request)
        {
            var dto = new OrganizationEditDto()
            {
                Id = request.Id,
                Name = request.Name,
                PictureName = request.PictureName,
                PictureFormat = request.PictureFormat,
                Comment = request.Comment,
                IsPictureDelete = request.IsPictureDelete,
                File = request.File,
            };

            // путь к папке Files
            string path = ""; //_appEnvironment.WebRootPath + "/Images/Org/";

            if (request.WorkToFile != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(request.WorkToFile.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)request.WorkToFile.Length);
                }

                dto.File = imageData;
                dto.PictureName = request.WorkToFile.FileName;
            }

            var result = await _service.EditItemAsync(dto, path);

            return Ok(result);
        }

        [HttpPost("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrganization(
            OrganizationDeleteModel request)
        {
            var dto = new OrganizationDeleteDto()
            {
                Id = request.Id,
                Name = request.Name,
                PictureName = request.PictureName,
                PictureFormat = request.PictureFormat,
            };

            string path = ""; //_appEnvironment.WebRootPath + "/Images/Org/";

            dto.PictureName = request.PictureName + "." + request.PictureFormat;
            var result = await _service.DeleteItemAsync(dto, path);

            return Ok(result);
        }
    }
}