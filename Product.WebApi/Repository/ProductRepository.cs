using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.WebApi.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Product.WebApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        ProductsContext _context;
        public ProductRepository(ProductsContext context)
        {
            _context = context;
        }

        public async Task Add(Models.Product item)
        {
            var owner = _context.ProductOwners.FirstOrDefault(x => x.OwnerId == item.Owner.OwnerId);
            item.Owner = owner;
            var producer = _context.Producers.FirstOrDefault(x => x.ManufacturerId == item.Producer.ManufacturerId);
            item.Producer = producer;
            await _context.Products.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Models.Product>> GetAll()
        {
            return await _context
                .Products
                .Include(p => p.Owner)
                .Include(p => p.Producer)
                .ToListAsync();
        }

        public async Task<Models.Product> Find(int id)
        {
            return await _context.Products
                .Where(e => e.ProductId == id)
                .SingleOrDefaultAsync();
        }

        public async Task Remove(int Id)
        {
            var itemToRemove = await _context.Products
                .SingleOrDefaultAsync(r => r.ProductId == Id);
            if (itemToRemove != null)
            {
                _context.Products.Remove(itemToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(Models.Product item)
        {
            var itemToUpdate = await _context.Products.
                                     SingleOrDefaultAsync(r => r.ProductId == item.ProductId);
            var owner = _context.ProductOwners.FirstOrDefault(x => x.OwnerId == item.Owner.OwnerId);
            var producer = _context.Producers.FirstOrDefault(x => x.ManufacturerId == item.Producer.ManufacturerId);

            if (itemToUpdate != null)
            {
                itemToUpdate.ProductName = item.ProductName;
                itemToUpdate.Description = item.Description;
                itemToUpdate.Price = item.Price;
                itemToUpdate.Owner = owner;
                itemToUpdate.Producer = producer;
                await _context.SaveChangesAsync();
            }
        }
    }
    
}
