using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Product.WebApi.Repository
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : DbContext
    {
        public T Context { get; }

        public UnitOfWork(T context)
        {
            Context = context;
        }
        public async Task<int> CommitAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public void Commit()
        {
            var count = Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
