using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace EAuction.Persistence.Repositories
{
    public interface IRepository<TEntity, TKey>
    {
        IQueryable<TEntity> Query();

        IAggregateFluent<TEntity> Aggregate();

        Task<TEntity> findByAsync(TKey key);

        Task<TEntity> AddAsync(TEntity entity);

        Task<bool> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TKey entity);
    }
}