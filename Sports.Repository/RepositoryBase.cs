using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Sports.Repository.Interface;
using Sports.Repository.Context;
using System.Linq;
using Sports.Repository.Exceptions;
using Sports.Repository.Constants;
using System.Data.SqlClient;
using System.Data.Common;

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
            try
            {
                return this.RepositoryContext.Set<T>().ToList<T>();
            }
            catch (InvalidOperationException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (DbException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (Exception)
            {
                throw new UnknownDatabaseException(AccessValidation.UNKNOWN_ERROR_MESSAGE);
            }
        }

        public ICollection<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            try
            {
                return this.RepositoryContext.Set<T>().Where(expression).ToList<T>();
            }
            catch (InvalidOperationException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (DbException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (Exception)
            {
                throw new UnknownDatabaseException(AccessValidation.UNKNOWN_ERROR_MESSAGE);
            }
        }

        public void Create(T entity)
        {
            try
            {
                this.RepositoryContext.Set<T>().Add(entity);
            }
            catch (InvalidOperationException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch(DbException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (Exception)
            {
                throw new UnknownDatabaseException(AccessValidation.UNKNOWN_ERROR_MESSAGE);
            }
        }

        public void Update(T entity)
        {
            try
            {
                this.RepositoryContext.Set<T>().Update(entity);
            }
            catch (InvalidOperationException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (DbException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (Exception)
            {
                throw new UnknownDatabaseException(AccessValidation.UNKNOWN_ERROR_MESSAGE);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                this.RepositoryContext.Set<T>().Remove(entity);
            }
            catch (InvalidOperationException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (DbException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (Exception)
            {
                throw new UnknownDatabaseException(AccessValidation.UNKNOWN_ERROR_MESSAGE);
            }
        }

        public void Save()
        {
            try
            {
                this.RepositoryContext.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                 throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (DbException)
            {
                throw new DisconnectedDatabaseException(AccessValidation.INVALID_ACCESS_MESSAGE);
            }
            catch (Exception)
            {
                throw new UnknownDatabaseException(AccessValidation.UNKNOWN_ERROR_MESSAGE);
            }
        }
    }
}
