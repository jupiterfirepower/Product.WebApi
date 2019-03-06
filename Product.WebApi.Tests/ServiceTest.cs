using System;
using System.Linq;
using Xunit;
using Product.WebApi.Repository;
using Product.WebApi.DataAccess;
using Product.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Product.WebApi.Mappings;
using Product.WebApi.Models;
using System.Collections.Generic;

namespace Product.WebApi.Tests
{
    public class ServiceTest
    {
        private IProductsService _service;
        private IUnitOfWork<ProductsContext> _ufw;
        private IMapper _mapper;

        public ServiceTest()
        {
            var context = new ProductsContext();
            _ufw = new UnitOfWork<ProductsContext>(context);
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
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

        [Fact]
        public void GetCategories_WhenCalled_MustReturnNotEmpty()
        {
            var data = _service.GetCategories();
            Assert.NotNull(data);
            var first = data.FirstOrDefault();
            Assert.True(data.Count() > 0);
            Assert.True(first.Children.Count() > 0);
        }

        [Fact]
        public void GetCategoriesMapping_WhenCalled_MustReturnNotEmpty()
        {
            var categories = _service.GetCategories();
            var data = _mapper.Map<IEnumerable<Category>, IList<CategoryDto>>(categories);
            var first = data.FirstOrDefault();
            Assert.NotNull(data);
            Assert.True(data.Count() > 0);
            Assert.True(first.Children.Count() > 0);
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


        [Fact]
        public void UpdateDifDBContext_WhenCalled_ExpectDbUpdateConcurrencyException()
        {
            var logFactory = new LoggerFactory();
            logFactory.AddProvider(new SqliteLoggerProvider());


            var context1 = new ProductsContext(new DbContextOptionsBuilder<ProductsContext>()
                .UseSqlite("Data Source=products.db").UseLoggerFactory(logFactory).Options);
            var context2 = new ProductsContext(new DbContextOptionsBuilder<ProductsContext>()
                .UseSqlite("Data Source=products.db").UseLoggerFactory(logFactory).Options);

            context1.Database.ExecuteSqlCommand(
                @"UPDATE Products SET RowVersion = randomblob(8) WHERE RowVersion = null");

            var productFromContext1 = context1.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == 1);
            var productFromContext2 = context1.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == 1);

            productFromContext1.Description = DateTime.Now.ToString();
            productFromContext2.Description = DateTime.UtcNow.ToString();

            try
            {
                context1.Entry(productFromContext1).State = EntityState.Modified;
                var count = context1.SaveChanges();
                productFromContext1 = context1.Products.FirstOrDefault(p => p.ProductId == 1);
                context2.Entry(productFromContext2).State = EntityState.Modified;
                count = context2.SaveChanges();
                Assert.True(false);
            }
            catch (DbUpdateConcurrencyException e)
            {
                Assert.True(true);
            }
            
        }
    }
}
