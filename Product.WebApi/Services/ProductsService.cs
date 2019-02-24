using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product.WebApi.Repository;
using Product.WebApi.DataAccess;

namespace Product.WebApi.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IUnitOfWork<ProductsContext> _uow;
        private readonly IRepository<Models.Product,ProductsContext> _repo;


        public ProductsService(IUnitOfWork<ProductsContext> unit, IRepository<Models.Product, ProductsContext> repo)
        {
            _uow = unit;
            _repo = repo;
        }

        public async Task<IEnumerable<Models.Product>> GetAll()
        {
            return await _repo.GetAll()
                .Include(p => p.Owner)
                .Include(p => p.Producer)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.User>> GetUsers()
        {
            return await _uow.Context.Users.ToListAsync();
        }

        public async Task<Models.Product> Find(int id)
        {
            return await _repo.Get(x=>x.ProductId==id)
                .Include(p => p.Owner)
                .Include(p => p.Producer)
                .Include(p => p.Category)
                .SingleOrDefaultAsync();
        }

        private void ChangeRelation(ref Models.Product item)
        {
            item.OwnerId = item.Owner?.OwnerId ?? item.OwnerId;
            item.ManufacturerId = item.Producer?.ManufacturerId ?? item.ManufacturerId;
            item.CategoryId = item.Category?.CategoryId ?? item.CategoryId;

            item.Owner = null;
            item.Producer = null;
            item.Category = null;
        }

        public async Task Add(Models.Product item)
        {
            ChangeRelation(ref item);

            item.ProductId = 0; //always new
            await _repo.Create(item);
            await _uow.CommitAsync();
        }

        public async Task Remove(Models.Product item)
        {
            await Remove(item.ProductId);
        }

        public async Task Remove(int id)
        {
            var current = await Find(id);
            if(current != null) await _repo.Remove(current.ProductId);
            await _uow.CommitAsync();
        }

        public async Task Update(Models.Product item)
        {
            ChangeRelation(ref item);

            await _repo.Change(item.ProductId, item);
            await _uow.CommitAsync();
        }
        
    }
}
