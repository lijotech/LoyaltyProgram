using MemberAPI.Data.Database;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MemberAPI.Data.Repository.v1
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _entities;
        public Repository(DbContext context)
        {
             _entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            try
            {

                return _entities.ToList();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't retrieve entities");
            }
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {   try
            {
                return await _entities.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't retrieve entities");
            }
        }
        public async Task<T> GetItem(T entityId)
        {
            try
            {
                if (entityId == null)
                {
                    throw new ArgumentNullException($"{nameof(GetItem)} entity must not be null");
                }

                var searchList = await _entities.FindAsync(entityId);

                return searchList;
            }
            catch (Exception)
            {
                throw new Exception("Couldn't retrieve entities");
            }
        }



        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {

                await _entities.AddAsync(entity);

                return entity;
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(entity)} could not be saved");
            }
        }

        public T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                _entities.Update(entity);

                return entity;
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(entity)} could not be updated");
            }
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }
            try
            {
                _entities.Remove(entity);
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(entity)} could not be updated");
            }
        }
        
    }
}