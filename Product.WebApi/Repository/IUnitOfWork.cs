using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.WebApi.Repository
{
    public interface IUnitOfWork<T> : IDisposable where T : DbContext
    {
        T Context { get; }
        Task<int> CommitAsync();
        void Commit();
    }
}
