using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.Core.Repositories;
using FoodOrderWeb.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderWeb.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected DataDbContext _context;
        protected DbSet<T> DbSet { get; set; }

        public Repository(DataDbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            var entry = await DbSet.AddAsync(entity);

            return entry.Entity;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }
        //identity
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual T GetById(int id)
        {
            return Queryable.FirstOrDefault(DbSet, e => e.Id == id);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }
    }
}
