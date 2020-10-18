using MemberAPI.Data.Database;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.Data.Repository.v1
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private bool _disposed;
        private readonly MemberContext _memberContext;
        public Repository(MemberContext memberContext)
        {
            _memberContext = memberContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {

                return _memberContext.Set<TEntity>();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't retrieve entities");
            }
        }
        public async Task<TEntity> GetItem(Guid entityId)
        {
            try
            {
                if (entityId == null)
                {
                    throw new ArgumentNullException($"{nameof(GetItem)} entity must not be null");
                }

                var searchList = await _memberContext.FindAsync<TEntity>(entityId);

                return searchList;
            }
            catch (Exception)
            {
                throw new Exception("Couldn't retrieve entities");
            }
        }



        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {

                await _memberContext.AddAsync(entity);
                //await _memberContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(entity)} could not be saved");
            }
        }

        public TEntity Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                _memberContext.Update(entity);
                //await _memberContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(entity)} could not be updated");
            }
        }

        public int SaveChanges()
        {
            int f= _memberContext.SaveChanges();
            _memberContext.Database.CommitTransaction();
            return f;
            
        }

        public async Task<int> SaveChangesAsync()
        {
            int f= await _memberContext.SaveChangesAsync();
            _memberContext.Database.CommitTransaction();
            return f;
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _memberContext.Database.BeginTransaction();

        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _memberContext.Database.BeginTransactionAsync();
        }

        protected void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _memberContext.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Rollback()
        {
            _memberContext.Database.RollbackTransaction();
            Dispose();
        }
    }
}