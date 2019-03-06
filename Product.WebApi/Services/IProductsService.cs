using Product.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.WebApi.Services
{
    public interface IProductsService 
    {
        Task Add(Models.Product item);
        Task<IEnumerable<Models.Product>> GetAll();
        Task<IEnumerable<Models.User>> GetUsers();
        IEnumerable<Category> GetCategories();
        Task<Models.Product> Find(int key);
        Task Remove(int Id);
        Task Update(Models.Product item);
    }
}
