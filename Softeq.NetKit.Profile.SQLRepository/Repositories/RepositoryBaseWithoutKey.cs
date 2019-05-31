// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Softeq.NetKit.Profile.Domain.Infrastructure;

namespace Softeq.NetKit.Profile.SQLRepository.Repositories
{
    public abstract class RepositoryBaseWithoutKey<TEntity> : IRepositoryWithoutKey<TEntity>
        where TEntity : class, new()
    {
        protected readonly DbSet<TEntity> _dbSet;

        protected RepositoryBaseWithoutKey(ApplicationDbContext dataContext)
        {
            Ensure.That(dataContext, "dataContext").IsNotNull();

            DataContext = dataContext;
            this._dbSet = this.DataContext.Set<TEntity>();
        }

        protected ApplicationDbContext DataContext { get; private set; }

        public virtual TEntity Add(TEntity entity)
        {
            Ensure.That(entity, "entity").IsNotNull();
            this._dbSet.Add(entity);
            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            Ensure.That(entity, "entity").IsNotNull();
            this._dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            Ensure.That(where, "where").IsNotNull();
            IEnumerable<TEntity> objects = this._dbSet.Where<TEntity>(where).AsEnumerable();
            foreach (TEntity obj in objects)
            {
                this._dbSet.Remove(obj);
            }
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return this._dbSet;
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            Ensure.That(where, "where").IsNotNull();
            return this._dbSet.Where(where);
        }

        public virtual void Update(TEntity entity)
        {
            Ensure.That(entity, "entity").IsNotNull();
            this._dbSet.Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
        }
    }
}