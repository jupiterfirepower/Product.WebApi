using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.WebApi.Repository
{
    public interface IProductRepository
    {
        Task Add(Models.Product item);
        Task<IEnumerable<Models.Product>> GetAll();
        Task<Models.Product> Find(int key);
        Task Remove(int Id);
        Task Update(Models.Product item);
    }
}
