using EAuction.Persistence.Entities;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace EAuction.Persistence.Repositories
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        protected readonly IMongoCollection<TEntity> collection;

        public Repository(IMongoDatabase mongoDatabase)
        {
            collection = mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual IQueryable<TEntity> Query()
        {
            return this.collection.AsQueryable();
        }

        public virtual IAggregateFluent<TEntity> Aggregate()
        {
            return this.collection.Aggregate();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await this.collection.InsertOneAsync(entity);
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(TKey key)
        {
            await this.collection.DeleteOneAsync(i => i.Id.Equals(key));
            return true;
        }

        public virtual async Task<TEntity> findByAsync(TKey key)
        {
            var result = await this.collection.FindAsync<TEntity>(i => i.Id.Equals(key));
            return result.FirstOrDefault();
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(i => i.Id, entity.Id);
            await this.collection.FindOneAndReplaceAsync(filter, entity);
            return true;
        }
    }
}