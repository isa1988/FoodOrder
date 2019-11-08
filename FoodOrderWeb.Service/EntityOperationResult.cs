using FoodOrderWeb.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderWeb.Service
{
    public class EntityOperationResult<T> where T : Entity
    {
        public bool IsSuccess { get; private set; }

        public T Entity { get; }

        public List<string> Errors { get; private set; }

        private EntityOperationResult(T entity)
        {
            Entity = entity;
        }

        private EntityOperationResult()
        {
        }

        public static EntityOperationResult<T> Success(T entity)
        {
            var result = new EntityOperationResult<T>(entity);
            result.IsSuccess = true;
            return result;
        }

        public static EntityOperationResult<T> Failure()
        {
            var result = new EntityOperationResult<T>();
            result.IsSuccess = false;
            result.Errors = new List<string>();

            return result;
        }

        public EntityOperationResult<T> AddError(params string[] errorMessages)
        {
            if (Errors == null)
            {
                Errors = new List<string>();
            }
            Errors.AddRange(errorMessages);

            return this;
        }
    }
}
