using System.Linq;
using Xunit;
using Product.WebApi.Repository;
using Product.WebApi.DataAccess;
using Product.WebApi.Services;

namespace Product.WebApi.Tests
{
    public class ServiceTest
    {
        private IProductsService _service;
        private IUnitOfWork<ProductsContext> _ufw;

        public ServiceTest()
        {
            var context = new ProductsContext();
            _ufw = new UnitOfWork<ProductsContext>(context);
            _service = new ProductsService(_ufw, new Repository<Models.Product, ProductsContext>(_ufw), new Repository<Models.User, ProductsContext>(_ufw));
        }

        // Service.Update method test
        [Fact]
        public async void Update_WhenCalled_CheckThatUpdated()
        {
            var product = new Models.Product { ProductId = 1, OwnerId = 1, CategoryId = 1, ProductName = "product updated", Description = "descr", Price = 1312.65m, ManufacturerId = 1, Owner = null, Producer = null };
            // Act
            await _service.Update(product);
            await _ufw.CommitAsync();

            var item = await _service.Find(product.ProductId);

            // Assert
            Assert.NotNull(item);
            Assert.True(item.ProductName == "product updated");
        }

        // Service.GetAll method test
        [Fact]
        public async void GetAll_WhenCalled_MustReturnNotEmpty()
        {
            var data = await _service.GetAll();
            Assert.NotNull(data);
            Assert.True(data.Count() > 0);
        }

        // Service.GetUsers method test
        [Fact]
        public async void GetUsers_WhenCalled_MustReturnNotEmpty()
        {
            var data = await _service.GetUsers();
            Assert.NotNull(data);
            Assert.True(data.Count() > 0);
        }

        // Service.GetAll method test
        [Fact]
        public async void Find_WhenCalled_MustReturnNotEmpty()
        {
            var data = await _service.Find(1);
            Assert.NotNull(data);
            Assert.True(data.ProductId == 1);
        }

        [Fact]
        public async void Remove_WhenCalled_MustReturnNotEmpty()
        {
            await _service.Remove(4);

            var item = await _service.Find(4);

            // Assert
            Assert.Null(item);
        }

        [Fact]
        public async void Add_WhenCalled_CheckThatAdded()
        {
            var product = new Models.Product { ProductId = 0, OwnerId = 1, ManufacturerId = 1, CategoryId = 1, ProductName = "product added", Description = "descr", Price = 1312.65m, Owner = null, Producer = null };
            // Act
            await _service.Add(product);
            await _ufw.CommitAsync();

            var item = await _service.Find(product.ProductId);

            // Assert
            Assert.NotNull(item);
            Assert.True(item.ProductName == "product added");
        }
    }
}
