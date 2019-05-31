// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Profile.Domain.Infrastructure;

namespace Softeq.NetKit.Profile.SQLRepository.Repositories
{
    public abstract class RepositoryBase<TEntity, TKey> : RepositoryBaseWithoutKey<TEntity>, IRepository<TEntity, TKey>
        where TEntity : class, IBaseEntity<TKey>, new()
    {
        protected RepositoryBase(ApplicationDbContext context)
            : base(context)
        {
        }

        public virtual TEntity GetById(TKey id)
        {
            return this._dbSet.Find(id);
        }
    }
}