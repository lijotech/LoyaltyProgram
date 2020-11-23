using MemberAPI.Data.Database;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MemberAPI.Data.Repository.v1
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal DbContext  _context;
        //private readonly IDbContext _context;
        internal DbSet<TEntity> _entities;
        public Repository(DbContext context)
        {
            this._context = context;
            this._entities = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
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
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _entities.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't retrieve entities");
            }
        }
        public virtual async Task<TEntity> GetItem(TEntity entityId)
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
        public virtual async Task<TEntity> AddAsync(TEntity entity)
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
        public virtual Task<TEntity> Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Update)} entity must not be null");
            }

            try
            {
                _entities.Update(entity);

                return Task.FromResult(entity);
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(entity)} could not be updated");
            }
        }
        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Delete)} entity must not be null");
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