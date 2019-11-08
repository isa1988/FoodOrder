using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.Core.Repositories;

namespace FoodOrderWeb.DAL.Unit.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IDishRepository Dish { get; }
        IOrganizationRepository Organization { get; }
        IBasketRepository Basket { get; }
        IBasketInventoryRepository BasketInventory { get; }
        IRatingRepository Ratings { get; }
        ICommentRepository Comments { get; }

    Task<int> CompleteAsync();
        void BeginTransaction();
        //void BeginTransaction(IsolationLevel level);
        void RollbackTransaction();
        void CommitTransaction();

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity;
    }
}
