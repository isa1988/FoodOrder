using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.DAL.Data;
using FoodOrderWeb.DAL.Unit;
using FoodOrderWeb.Service.Dtos.Basket;
using FoodOrderWeb.Service.Dtos.BasketInventory;
using FoodOrderWeb.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class Tests
    {
        public static string ConnectionString = "Server=(localdb)\\mssqllocaldb;AttachDBFilename=C:\\Projects\\isa-project\\FoodOrder\\FoodOrderWeb\\ContentRootPath\\FoodOrder.mdf;Trusted_Connection=true;MultipleActiveResultSets=true";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task SuccessResponceIfOrderCreated()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseSqlServer(ConnectionString, null)
                .Options;
            var dbContextFactory = new DataDbContextFactory(options);
            var unitOfWorkFactory = new UnitOfWorkFactory(dbContextFactory);
            var service = new BasketService(unitOfWorkFactory);

            var firstItem = new BasketInventoryCreateDto()
            {
                DishId = 4,
                CountInventory = 1,
                Price = 300,
                Sum = 300
            };

            var secondItem = new BasketInventoryCreateDto()
            {
                DishId = 5,
                CountInventory = 1,
                Price = 100,
                Sum = 100
            };

            var basket = new BasketCreateDto()
            {
                UserId = 2,
                OrganizationId = 2,
                BasketInventoryDto = new List<BasketInventoryCreateDto>()
                {
                    firstItem,
                    secondItem
                }
            };

            var resultBasket = await service.CreateItemAsync(basket);

            Assert.AreEqual(true, resultBasket.IsSuccess);

        }
    }
}