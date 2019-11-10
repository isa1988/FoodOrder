using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FoodOrderWeb.Core.DataBase;

using FoodOrderWeb.DAL.Unit.Contracts;
using FoodOrderWeb.Service.Dtos.Organization;
using FoodOrderWeb.Service.Services.Contracts;
using FoodOrderWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderWeb.Controllers
{
    public class OrganizationController : BaseController
    {
        private  readonly IOrganizationService _service;

        private IHostingEnvironment _appEnvironment;

        public OrganizationController(IUnitOfWorkFactory unitOfWorkFactory, IHostingEnvironment appEnvironment,
                                      IOrganizationService service) : base(unitOfWorkFactory)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (appEnvironment == null)
                throw new ArgumentNullException(nameof(appEnvironment));

            _service = service;
            _appEnvironment = appEnvironment;
        }

        // GET: Organization
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var organizations = unitOfWork.Organization.GetAll();
                var model = new ListModel
                {
                    Organizations = organizations
                };
                return View(model);
            }
        }

        // GET: Organization
        [Authorize(Roles = "User")]
        public async Task<IActionResult> IndexOrg()
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var organizations = unitOfWork.Organization.GetAll();
                var model = new ListModel
                {
                    Organizations = organizations
                };
                return View(model);
            }
        }

        public async Task<IActionResult> IndexAll()
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var organizations = unitOfWork.Organization.GetAll();
                var model = new ListModel
                {
                    Organizations = organizations
                };
                return View(model);
            }
        }

        // GET: Organization/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new OrganizationCreatingModel{Title = "Создание"});
        }

        // POST: Organization/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizationCreatingModel request)
        {
            var dto = Mapper.Map<OrganizationCreateDto>(request);

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
                path = _appEnvironment.WebRootPath + "/Images/Org/";
            }

            var result = await _service.CreateItemAsync(dto, path);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
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

        // GET: Organization/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (!id.HasValue)
                {
                    return NotFound();
                }

                var organization = unitOfWork.Organization.GetById(id.Value);

                var model = Mapper.Map<OrganizationEditModel>(organization);
                
                return View(model);
            }
        }

        // POST: Organization/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrganizationEditModel request)
        {
            var dto = Mapper.Map<OrganizationEditDto>(request);

            // путь к папке Files
            string path = _appEnvironment.WebRootPath + "/Images/Org/";

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
                return RedirectToAction("Index");
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

        // GET: Organization/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (!id.HasValue)
                {
                    return NotFound();
                }

                var organization = unitOfWork.Organization.GetById(id.Value);

                var model = Mapper.Map<OrganizationDeleteModel>(organization);

                return View(model);
            }
        }

        // POST: Organization/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(OrganizationDeleteModel request)
        {
            var dto = Mapper.Map<OrganizationDeleteDto>(request);

            // путь к папке Files
            string path = _appEnvironment.WebRootPath + "/Images";
            dto.PictureName = request.PictureName + "." + request.PictureFormat;
            var result = await _service.DeleeteItemAsync(dto, path);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
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