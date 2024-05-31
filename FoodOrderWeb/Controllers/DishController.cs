using FoodOrderWeb.DAL.Unit.Contracts;
using FoodOrderWeb.Models;
using FoodOrderWeb.Models.Dish;
using FoodOrderWeb.Service.Dtos.Dish;
using FoodOrderWeb.Service.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : BaseController
    {
        private readonly IDishService _service;

        public DishController(
            IUnitOfWorkFactory unitOfWorkFactory,
            IDishService service) : 
            base(unitOfWorkFactory)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            _service = service;
        }

        [HttpGet("Get/{organizationId}")]
        public async Task<IActionResult> GetDishes(int? organizationId)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (!organizationId.HasValue)
                {
                    var dishes = unitOfWork.Dish.GetAll();
                    var model = new ListModel
                    {
                        Dishes = dishes
                    };
                    return View(model);
                }
                else
                {
                    var dishes = unitOfWork.Dish.GetAll(organizationId.Value);
                    var model = new ListModel
                    {
                        Dishes = dishes,
                        OrganizationId = organizationId
                    };
                    return View(model);
                }
            }
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDish(DishCreatingModel request)
        {
            var dto = new DishCreateDto()
            {
                Name = request.Name,
                Price = request.Price,
                OrganizationId = request.OrganizationId,
                PictureName = request.PictureName,
                PictureFormat = request.PictureFormat,
                File = request.File,
                Comment = request.Comment,
            };

            // путь к папке Files
            string path = string.Empty;

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
                path = ""; //_appEnvironment.WebRootPath + "/Images/Dish/";
            }

            var result = await _service.CreateItemAsync(dto, path);

            return Ok(result);
        }

        [HttpPost("Edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditDish(DishEditModel request)
        {
            var dto = new DishEditDto()
            {
                Id = request.Id,
                Name = request.Name,
                Price = request.Price,
                OrganizationId = request.OrganizationId,
                PictureName = request.PictureName,
                PictureFormat = request.PictureFormat,
                File = request.File,
                IsPictureDelete = request.IsPictureDelete,
                Comment = request.Comment,
            };

            string path = ""; //_appEnvironment.WebRootPath + "/Images/Dish/";

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
        public async Task<IActionResult> DeleteDish(DishDeleteModel request)
        {
            var dto = new DishDeleteDto() 
            { 
                Id = request.Id,
                Name = request.Name,
                Price = request.Price,
                PictureName = request.PictureName,
                PictureFormat = request.PictureFormat,
                Comment = request.Comment,
            };

            int organizationIdBeforDelete = request.OrganizationId;
            // путь к папке Files
            string path = ""; // _appEnvironment.WebRootPath + "/Images/Dish/";
            dto.PictureName = request.PictureName + "." + request.PictureFormat;
            var result = await _service.DeleteItemAsync(dto, path);
            return Ok(result);
        }
    }
}
