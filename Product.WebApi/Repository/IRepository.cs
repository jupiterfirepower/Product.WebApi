using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Product.WebApi.Repository
{
    public interface IRepository<T, C> where T : class where C : DbContext
    {
        IQueryable<T> GetAll();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        Task Create(T entity);
        Task Change(object primaryKeyId, T entity);
        Task Remove(params object[] keyValues);
        Task Remove(int id);
        Task Remove(string Id);
        Task Remove(Guid Id);

        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
