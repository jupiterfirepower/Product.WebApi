using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using Product.WebApi.Repository;
using Product.WebApi.Controllers;
using Product.WebApi.DataAccess;
using Product.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Product.WebApi.Services;
using AutoMapper;
using Product.WebApi.Mappings;

namespace Product.WebApi.Tests
{
    public class ProductControllerTest
    {
        private ProductsController _controller;
        private IProductsService _service;

        public ProductControllerTest()
        {
            var context = new ProductsContext();
            var ufw = new UnitOfWork<ProductsContext>(context);
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            _service = new ProductsService(ufw, new Repository<Models.Product, ProductsContext>(ufw), new Repository<Models.User, ProductsContext>(ufw));
            _controller = new ProductsController(_service, mapper);
        }

        // GetAll tests
        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.GetAll();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.GetAll().Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Models.ProductDto>>(okResult.Value);
            Assert.True(items.Count > 0);
        }

        // GetById tests
        [Fact]
        public void GetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = _controller.GetById(-1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetById_ExistingIdPassed_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.GetById(1);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetById_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var okResult = _controller.GetById(1).Result as OkObjectResult;

            // Assert
            Assert.IsType<Models.ProductDto>(okResult.Value);
            Assert.Equal(1, (okResult.Value as Models.ProductDto).ProductId);
        }

        // Add tests - POST
        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            var owner1 = new ProductOwnerDto
            {
                OwnerId = 1, OwnerName = "owner 1", Address = "owner address 1", Email = "owner1@owners.net",
                Phone = "213421412341"
            };

            var producer = new ProducerDto()
                { ProducerId = 1, ProducerName = "Manufacturer 1", ProducerAddress = "Manufacturer address 1"};

            var category = new CategoryDto()
                { CategoryId = 1, Name = "Category 1", Description = "Category 1", Parent = null, ParentId = null };

            var product = new Models.ProductDto()
            {
                ProductId = 1,
                ProductName = "product name test",
                Description = "product test description",
                Price = 231.33424m,
                Owner = owner1,
                Producer = producer,
                Category = category,
                RowVersion = null
            };

            _controller.ModelState.AddModelError("ProductName", "Required");
            // Act
            var badResponse = _controller.Create(product);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var owner1 = new ProductOwnerDto
            {
                OwnerId = 1, OwnerName = "owner 1", Address = "owner address 1", Email = "owner1@owners.net",
                Phone = "213421412341"
            };

            var producer = new ProducerDto()
                { ProducerId = 1, ProducerName = "Manufacturer 1", ProducerAddress = "Manufacturer address 1" };

            var category = new CategoryDto()
                { CategoryId = 1, Name = "Category 1", Description = "Category 1", Parent = null, ParentId = null };

            var product = new Models.ProductDto()
            {
                ProductId = 7, ProductName = "product name test", Description = "product test description",
                Price = 231.33424m,
                Owner = owner1,
                Producer = producer,
                Category = category
            };

            // Act
            var createdResponse = _controller.Create(product).Result;

            // Assert
            Assert.IsType<CreatedAtRouteResult>(createdResponse);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var owner1 = new ProductOwnerDto
            {
                OwnerId = 1,
                OwnerName = "owner 1",
                Address = "owner address 1",
                Email = "owner1@owners.net",
                Phone = "213421412341"
            };

            var producer = new ProducerDto()
                { ProducerId = 1, ProducerName = "Manufacturer 1", ProducerAddress = "Manufacturer address 1" };

            var category = new CategoryDto()
                { CategoryId = 1, Name = "Category 1", Description = "Category 1", Parent = null, ParentId = null };

            var product = new Models.ProductDto()
            {
                ProductId = 7,
                ProductName = "product name test special",
                Description = "product test description",
                Price = 231.33424m,
                Owner = owner1,
                Producer = producer,
                Category = category
            };

            if (!(_controller.GetById(product.ProductId).Result as NotFoundResult != null))
            {
                var okResponse = _controller.Delete(product.ProductId);
                // Assert
                Assert.IsType<OkResult>(okResponse.Result);
            }

            // Act
            var t = _controller.Create(product).Result;
            var createdResponse = _controller.Create(product).Result as CreatedAtRouteResult;
            var item = createdResponse.Value as Models.ProductDto;

            // Assert
            Assert.IsType<Models.ProductDto>(item);
            Assert.Equal("product name test special", item.ProductName);
        }

        [Fact]
        public void Update_InvalidObjectPassed_ReturnsNotFoundRequest()
        {
            var owner1 = new Models.ProductOwnerDto
            {
                OwnerId = 1,
                OwnerName = "owner 1",
                Address = "owner address 1",
                Email = "owner1@owners.net",
                Phone = "213421412341"
            };

            var producer = new ProducerDto()
                { ProducerId = 1, ProducerName = "Manufacturer 1", ProducerAddress = "Manufacturer address 1" };

            var product = new Models.ProductDto()
            {
                ProductId = 9,
                ProductName = "product name updated",
                Description = "product test updated",
                Price = 231.33424m,
                Owner = owner1,
                Producer = producer
            };

            // Act
            var createdResponse = _controller.Update(product.ProductId, product);

            // Assert
            Assert.IsType<NotFoundResult>(createdResponse.Result);
        }

        [Fact]
        public void Update_InvalidObjectPassed_ReturnsBadRequest()
        {
            var owner1 = new ProductOwnerDto
            {
                OwnerId = 1,
                OwnerName = "owner 1",
                Address = "owner address 1",
                Email = "owner1@owners.net",
                Phone = "213421412341"
            };

            var producer = new ProducerDto()
                { ProducerId = 1, ProducerName = "Manufacturer 1", ProducerAddress = "Manufacturer address 1" };

            var category = new CategoryDto()
                { CategoryId = 1, Name = "Category 1", Description = "Category 1", Parent = null, ParentId = null };

            var product = new Models.ProductDto()
            {
                ProductId = 9,
                ProductName = "product name updated",
                Description = "product test updated",
                Price = 231.33424m,
                Owner = owner1,
                Producer = producer,
                Category = category
            };

            _controller.ModelState.AddModelError("ProductName", "Required");
            // Act
            var createdResponse = _controller.Update(product.ProductId, product);

            // Assert
            Assert.IsType<BadRequestObjectResult>(createdResponse.Result);
        }

        [Fact]
        public void Update_ValidObjectPassed_ReturnedResponseOkResult()
        {
            // Arrange
            var owner1 = new ProductOwnerDto
            {
                OwnerId = 1,
                OwnerName = "owner 1",
                Address = "owner address 1",
                Email = "owner1@owners.net",
                Phone = "213421412341"
            };
            var producer = new ProducerDto()
                { ProducerId = 1, ProducerName = "Manufacturer 1", ProducerAddress = "Manufacturer address 1" };

            var category = new CategoryDto()
                { CategoryId = 1, Name = "Category 1", Description = "Category 1", Parent = null, ParentId = null };

            var product = new Models.ProductDto()
            {
                ProductId = 2,
                ProductName = "product name test special",
                Description = "product test description",
                Price = 231.33424m,
                Owner = owner1,
                Producer = producer,
                Category = category
            };

            // Act
            var createdResponse = _controller.Update(product.ProductId, product);

            // Assert
            Assert.IsType<OkResult>(createdResponse.Result);
        }

        // REMOVE tests - DELETE
        [Fact]
        public void Remove_NotExistingIdPassed_ReturnsNotFoundResponse()
        {
            // Act
            var badResponse = _controller.Delete(-1);

            // Assert
            Assert.IsType<NotFoundResult>(badResponse.Result);
        }

        [Fact]
        public void Remove_ExistingIdPassed_ReturnsOkResult()
        {
            // Act
            var okResponse = _controller.Delete(3);

            // Assert
            Assert.IsType<OkResult>(okResponse.Result);
        }
    }
}
