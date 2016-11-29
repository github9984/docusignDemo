using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using docusignDemo.Data.Entities;

namespace docusignDemo.Data
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal docusignDemoEntities Context;

        internal DbSet<TEntity> DbSet;

        public GenericRepository(docusignDemoEntities context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual int Count()
        {
            IQueryable<TEntity> query = DbSet;
            return query.Count();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int takeCount = 0)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            query = includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
                return takeCount != 0 ? orderBy(query).Take(takeCount).ToList() : orderBy(query).ToList();
            return takeCount != 0 ? query.Take(takeCount).ToList() : query.ToList();
        }


        public virtual TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            query = includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));


            return query.FirstOrDefault();
        }

        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }


        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }


        public virtual void Delete(object id)
        {
            var entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }


        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
                DbSet.Attach(entityToDelete);
            DbSet.Remove(entityToDelete);
        }


        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}