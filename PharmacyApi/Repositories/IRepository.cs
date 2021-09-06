using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApi.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        public IQueryable<TEntity> GetAll();
        public Task<TEntity> AddAsync(TEntity entity);
        public Task<TEntity> UpdateAsync(TEntity entity);
        public Task<TEntity> DeleteAsync(TEntity entity);
        
    }
}