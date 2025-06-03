using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T:class
    {
        IEnumerable<T> GetAll();

        IQueryable<T> GetAllAlongWithLinked();
        T Get(Expression<Func<T, bool>> filter);

        IEnumerable<T> GetList(Expression<Func<T, bool>> filter);

        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
