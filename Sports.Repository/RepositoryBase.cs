using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Sports.Repository.Interface;
using Sports.Repository.Context;
using System.Linq;
using Sports.Repository.Exceptions;
using Sports.Repository.Constants;

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
            try
            {
                this.RepositoryContext.Set<T>().Add(entity);
            }
            catch (Exception)
            {
                throw new InvalidDatabaseAccessException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            this.Save();
        }

        public void Update(T entity)
        {
            try
            {
                this.RepositoryContext.Set<T>().Update(entity);
            }
            catch (Exception)
            {
                throw new InvalidDatabaseAccessException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            this.Save();
        }

        public void Delete(T entity)
        {
            try
            {
                this.RepositoryContext.Set<T>().Remove(entity);
            }
            catch (Exception)
            {
                throw new InvalidDatabaseAccessException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            this.Save();
        }
        
        public void Save()
        {
            try
            {
                this.RepositoryContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new InvalidDatabaseAccessException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            
        }
    }
}
