using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Product.WebApi.Repository
{
    public class Repository<T, C> : IRepository<T, C> 
        where T : class  
        where C : DbContext
    {
        private readonly IUnitOfWork<C> _unitOfWork;

        public Repository(IUnitOfWork<C> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(T entity)
        {
            _unitOfWork.Context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(entity);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
        }

        public IQueryable<T> GetAll()
        {
            return _unitOfWork.Context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _unitOfWork.Context.Set<T>().Where(predicate).AsNoTracking();
        }

        public void Update(T entity)
        {
            _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
            _unitOfWork.Context.Set<T>().Attach(entity);
        }

        public async Task Create(T entity)
        {
            await _unitOfWork.Context.Set<T>().AddAsync(entity);
        }

        /*public async Task Change(object primaryKeyId, T entity)
        {
            var modifiedEntities = _unitOfWork.Context.ChangeTracker.Entries()
                .Where(p => p.Entity as T != null).ToList();

            bool finded = false;

            foreach (var change in modifiedEntities)
            {
                var primaryKey = change.Metadata.FindPrimaryKey();
                var keys = primaryKey.Properties.ToDictionary(x => x.Name, x => x.PropertyInfo.GetValue(change.Entity));

                if (keys.FirstOrDefault().Value.ToString().Equals(primaryKeyId.ToString()))
                {
                    _unitOfWork.Context.Entry(change.Entity).CurrentValues.SetValues(entity);
                    finded = true;
                    break;
                }
            }

            if (!finded)
            {
                T existing = _unitOfWork.Context.Set<T>().Find(primaryKeyId);
                if (existing != null) _unitOfWork.Context.Entry(existing).CurrentValues.SetValues(entity);

                //_unitOfWork.Context.Entry(entity).State = EntityState.Modified;
                //_unitOfWork.Context.Set<T>().Attach(entity);
            }
            
            await Task.Delay(0);
        }*/

        public async Task Change(object primaryKeyId, T entity)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(primaryKeyId);
            if (existing != null) _unitOfWork.Context.Entry(existing).CurrentValues.SetValues(entity);

            await Task.Delay(0);
        }

        private async Task Remove(object keyValue)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(keyValue);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
            await Task.Delay(0);
        }

        public async Task Remove(int id)
        {
            await Remove(id as object);
        }

        public async Task Remove(string id)
        {
            await Remove(id as object);
        }

        public async Task Remove(Guid id)
        {
            await Remove(id as object);
        }

        public async Task Remove(params object[] keyValues)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(keyValues);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
            await Task.Delay(0);
        }
    }
}
