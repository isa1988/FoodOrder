using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FoodOrderWeb.DAL.Unit.Contracts;
using FoodOrderWeb.Service.Dtos.Dish;
using FoodOrderWeb.Service.Services.Contracts;
using FoodOrderWeb.Models;
using FoodOrderWeb.Models.Dish;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodOrderWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DishController : BaseController
    {
        private readonly IDishService _service;

        private IHostingEnvironment _appEnvironment;

        public DishController(IUnitOfWorkFactory unitOfWorkFactory, IHostingEnvironment appEnvironment,
            IDishService service) : base(unitOfWorkFactory)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (appEnvironment == null)
                throw new ArgumentNullException(nameof(appEnvironment));

            _service = service;
            _appEnvironment = appEnvironment;
        }

        // GET: Organization
        public async Task<IActionResult> Index(int? organizationId)
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

        // GET: Organization/Create
        public IActionResult Create(int? organizationId)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var model = new DishCreatingModel
                {
                    Title = "Создание"
                };
                if (!organizationId.HasValue)
                {
                    model.OrganizationList = new SelectList(unitOfWork.Organization.GetAll(), "Id", "Name");
                }
                else
                {
                    model.OrganizationList = new SelectList(unitOfWork.Organization.GetAll(), "Id", "Name", organizationId);
                }
                return View(model);
            }
        }

        // POST: Organization/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DishCreatingModel request)
        {
            var dto = Mapper.Map<DishCreateDto>(request);

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
                path = _appEnvironment.WebRootPath + "/Images/Dish/";
            }

            var result = await _service.CreateItemAsync(dto, path);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index", new { organizationId = request.OrganizationId});
            }
            else
            {
                using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
                {
                    foreach (var resultError in result.Errors)
                    {
                        ModelState.AddModelError("Error", resultError);
                    }

                    request.OrganizationList = new SelectList(unitOfWork.Organization.GetAll(), "Id", "Name", request.OrganizationId);
                    return View(request);
                }
            }
        }

        // GET: Organization/Edit/5
        public IActionResult Edit(int? id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (!id.HasValue)
                {
                    return NotFound();
                }

                var organization = unitOfWork.Dish.GetById(id.Value);

                var model = Mapper.Map<DishEditModel>(organization);
                model.OrganizationList = new SelectList(unitOfWork.Organization.GetAll(), "Id", "Name", model.OrganizationId);
                return View(model);
            }
        }

        // POST: Organization/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DishEditModel request)
        {
            var dto = Mapper.Map<DishEditDto>(request);

            // путь к папке Files
            string path = _appEnvironment.WebRootPath + "/Images/Dish/";

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

            if (result.IsSuccess)
            {
                return RedirectToAction("Index", new { organizationId = request.OrganizationId });
            }
            else
            {
                using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
                {
                    foreach (var resultError in result.Errors)
                    {
                        ModelState.AddModelError("Error", resultError);
                    }

                    request.OrganizationList = new SelectList(unitOfWork.Organization.GetAll(), "Id", "Name",
                        request.OrganizationId);
                    return View(request);
                }
            }
        }

        // GET: Organization/Delete/5
        public IActionResult Delete(int? id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (!id.HasValue)
                {
                    return NotFound();
                }

                var organization = unitOfWork.Dish.GetById(id.Value);

                var model = Mapper.Map<DishDeleteModel>(organization);

                return View(model);
            }
        }

        // POST: Organization/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DishDeleteModel request)
        {
            var dto = Mapper.Map<DishDeleteDto>(request);
            int organizationIdBeforDelete = request.OrganizationId;
            // путь к папке Files
            string path = _appEnvironment.WebRootPath + "/Images/Dish/";
            dto.PictureName = request.PictureName + "." + request.PictureFormat;
            var result = await _service.DeleeteItemAsync(dto, path);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index", new { organizationId = organizationIdBeforDelete });
            }
            else
            {
                foreach (var resultError in result.Errors)
                {
                    ModelState.AddModelError("Error", resultError);
                }
                return View(request);
            }
        }
    }
}
