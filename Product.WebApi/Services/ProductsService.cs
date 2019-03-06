using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product.WebApi.Repository;
using Product.WebApi.DataAccess;
using System.Linq;
using AutoMapper;
using Product.WebApi.Models;

namespace Product.WebApi.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IUnitOfWork<ProductsContext> _uow;
        private readonly IRepository<Models.Product,ProductsContext> _productRepository;
        private readonly IRepository<Models.User, ProductsContext> _userRepository;

        public ProductsService(IUnitOfWork<ProductsContext> unit, IRepository<Models.Product, ProductsContext> productRepository, IRepository<Models.User, ProductsContext> userRepository)
        {
            _uow = unit;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Models.Product>> GetAll()
        {
            return await _productRepository.GetAll()
                .Include(p => p.Owner)
                .Include(p => p.Producer)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.User>> GetUsers()
        {
            return await _userRepository.GetAll().ToListAsync();
        }

        public IEnumerable<Category> GetCategories()
        {
            var categories = _uow.Context.Categories
                .Include(x => x.Children)
                .AsEnumerable()
                .Where(x => x.Parent == null)
                .ToList();
            return categories;
        }

        public async Task<Models.Product> Find(int id)
        {
            return await _productRepository.Get(x=>x.ProductId==id)
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

            item.ProductId = 0; //always new ignore original ProductId from product entity
            await _productRepository.Create(item);
            await _uow.CommitAsync();
        }

        public async Task Remove(Models.Product item)
        {
            await Remove(item.ProductId);
        }

        public async Task Remove(int id)
        {
            var current = await Find(id);
            if(current != null) await _productRepository.Remove(current.ProductId);
            await _uow.CommitAsync();
        }

        public async Task Update(Models.Product item)
        {
            ChangeRelation(ref item);

            await _productRepository.Change(item.ProductId, item);
            await _uow.CommitAsync();
        }
        
    }
}
