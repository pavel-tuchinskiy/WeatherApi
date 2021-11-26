using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly WeatherDbContext context;
        private readonly IMemoryCache cache;

        protected RepositoryBase(WeatherDbContext context, IMemoryCache cache)
        {
            this.context = context;
            this.cache = cache;
        }

        protected RepositoryBase(WeatherDbContext context)
        {
            this.context = context;
        }

        public void Create(T entity) => context.Set<T>().Add(entity);

        public void Delete(T entity) => context.Set<T>().Remove(entity);

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ? context.Set<T>().AsNoTracking() : context.Set<T>();

        public IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            bool trackChanges)
        {
            IQueryable<T> query = context.Set<T>();

            if (!trackChanges)
                query.AsNoTracking();

            if (include != null)
                query = include(query);

            return query;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ? context.Set<T>().Where(expression).AsNoTracking() : context.Set<T>().Where(expression);

        public IQueryable<T> FindByCondition(Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            Expression<Func<T, bool>> expression,
            bool trackChanges)
        {
            IQueryable<T> query = context.Set<T>();

            if (!trackChanges)
                query.AsNoTracking();

            if (include != null)
                query = include(query);

            if (expression != null)
                query = query.Where(expression);

            return query; ;
        }

        public void Update(T entity) => context.Set<T>().Update(entity);
    }
}
