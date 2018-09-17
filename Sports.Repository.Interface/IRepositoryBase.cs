using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Sports.Repository.Interface
{
    public interface IRepositoryBase<T>
    {
        ICollection<T> FindAll();
        ICollection<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
