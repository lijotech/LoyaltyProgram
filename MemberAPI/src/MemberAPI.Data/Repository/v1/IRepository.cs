using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberAPI.Data.Repository.v1{

    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> AddAsync(TEntity entity);

        TEntity Update(TEntity entity);

        Task<TEntity> GetItem(TEntity entityId);

        void Delete(TEntity entity);

    }
}