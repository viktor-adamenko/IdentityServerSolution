using PharmacyApi.Data;
using PharmacyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApi.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly PharmacyDbContext _dbContext;
        public Repository(PharmacyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");

            try
            {
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();

                return entity;
            } 
            catch(Exception ex)
            {
                throw new Exception($"Coudn't create entity: ${ex.Message}");
            }
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException($"{nameof(DeleteAsync)} entity must not be null");

            try
            {
                _dbContext.Remove(entity);
                await _dbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Coudn't delete entity: ${ex.Message}");
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return _dbContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Coudn't retrieve entities: ${ex.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException($"{nameof(UpdateAsync)} entity must not be null");

            try
            {
                _dbContext.Update(entity);
                await _dbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Coudn't update entity: ${ex.Message}");
            }
        }
    }
}
