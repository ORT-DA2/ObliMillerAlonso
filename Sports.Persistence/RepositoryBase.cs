using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Sports.Repository.Interface;
using Sports.Persistence.Context;
using System.Linq;

namespace Sports.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public ICollection<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().ToList<T>();
        }

        public ICollection<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression).ToList<T>();
        }

        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
            this.Save();
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }

        public void Save()
        {
            this.RepositoryContext.SaveChanges();
        }
    }
}
