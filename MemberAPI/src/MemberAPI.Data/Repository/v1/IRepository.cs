using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberAPI.Data.Repository.v1{

    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> AddAsync(T entity);

        T Update(T entity);

        Task<T> GetItem(T entityId);

        void Delete(T entity);

    }
}