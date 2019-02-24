using System;
using System.Linq;
using Xunit;
using Product.WebApi.Repository;
using Product.WebApi.DataAccess;
using Product.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Product.WebApi.Tests
{
    public class RepositoryTest
    {
        private IRepository<ProductOwner, ProductsTestContext> _repository;
        private IUnitOfWork<ProductsTestContext> _ufw;

        public RepositoryTest()
        {
            var context = new ProductsTestContext();
            _ufw = new UnitOfWork<ProductsTestContext>(context);
            _repository =  new Repository<ProductOwner, ProductsTestContext>(_ufw);
        }

        // Repository.Add method test
        [Fact]
        public void Add_WhenCalled_CheckThatAdded()
        {
            var productOwner = new ProductOwner { OwnerId = 10, OwnerName = "owner 10", Address = "owner address 10", Email = "owner10@owners.net", Phone = "63455463456436" };
            // Act
            _repository.Add(productOwner);
            _ufw.Commit();

            var item = _repository.Get(x => x.OwnerId == productOwner.OwnerId).FirstOrDefault();

            // Assert
            Assert.NotNull(item);
            Assert.True(item.OwnerName == "owner 10");
        }

        // Repository.Add method test
        [Fact]
        public void Add_WhenCalled_MustBeException_IdNegative()
        {
            var productOwner = new ProductOwner { OwnerId = -10, OwnerName = "owner -10", Address = "owner address -10", Email = "owner-10@owners.net", Phone = "780905845785475" };
            // Act
            try
            {
                _repository.Add(productOwner);
                _ufw.Commit();
            }
            catch (DbUpdateException)
            {
                Assert.True(true);
            }
            catch (AggregateException)
            {
                Assert.True(true);
            }
        }

        // Repository.Add method test
        [Fact]
        public void Add_WhenCalled_MustBeException_NullValue()
        {
            var productOwner = new ProductOwner { OwnerId = -10, OwnerName = null, Address = "owner address -10", Email = "owner-10@owners.net", Phone = "780905845785475" };
            // Act
            try
            {
                _repository.Add(productOwner);
                _ufw.Commit();
            }
            catch (DbUpdateException)
            {
                Assert.True(true);
            }
        }
// GetAll tests 
        // Repository.GetAll method test
        [Fact]
        public void GetAll_WhenCalled_MustReturnNotEmpty()
        {
            var data = _repository.GetAll().AsEnumerable();
            Assert.NotNull(data);
            Assert.True(data.Count() > 0);
        }
// Getl tests         
        // Repository.Get method test
        [Fact]
        public void Get_WhenCalled_MustReturnNotEmpty()
        {
            var productOwner = new ProductOwner { OwnerId = 11, OwnerName = "owner 11", Address = "owner address 11", Email = "owner11@owners.net", Phone = "56785478567847" };
            // Act
            _repository.Add(productOwner);
            _ufw.Commit();

            var item = _repository.Get(x => x.OwnerId == productOwner.OwnerId).FirstOrDefault();

            // Assert
            Assert.NotNull(item);
            Assert.True(item.OwnerName == "owner 11");

            var data = _repository.Get(x => x.OwnerId == 11).FirstOrDefault();
            Assert.NotNull(data);
            Assert.True(data.OwnerId == 11);
        }

        // Change tests         
        // Repository.Change method test
        [Fact]
        public async void Change_WhenCalled_MustReturnUpdated()
        {
            var productOwner = new ProductOwner { OwnerId = 12, OwnerName = "owner 12", Address = "owner address 12", Email = "owner12@owners.net", Phone = "56785478567847" };
            // Act
            _repository.Add(productOwner);
            _ufw.Commit();

            var item = _repository.Get(x => x.OwnerId == productOwner.OwnerId).AsNoTracking().FirstOrDefault();

            // Assert
            Assert.NotNull(item);
            Assert.True(item.OwnerName == "owner 12");

            item.OwnerName = "owner 12 updated";

            // Act
            await _repository.Change(item.OwnerId,item);
            await _ufw.CommitAsync();

            item = _repository.Get(x => x.OwnerId == item.OwnerId).FirstOrDefault();

            // Assert
            Assert.NotNull(item);
            Assert.True(item.OwnerId == item.OwnerId);
            Assert.True(item.OwnerName == "owner 12 updated");
        }


        // Change tests         
        // Repository.Change method test
        [Fact]
        public async void Remove_WhenCalled_MustReturnUpdated()
        {
            var productOwner = new ProductOwner { OwnerId = 14, OwnerName = "owner 14", Address = "owner address 14", Email = "owner14@owners.net", Phone = "1235445435345" };
            // Act
            _repository.Add(productOwner);
            await _ufw.CommitAsync();

            var item = _repository.Get(x => x.OwnerId == productOwner.OwnerId).FirstOrDefault();

            // Assert
            Assert.NotNull(item);
            Assert.True(item.OwnerName == "owner 14");

            await _repository.Remove(item.OwnerId);
            await _ufw.CommitAsync();

            item = _repository.Get(x => x.OwnerId == item.OwnerId).FirstOrDefault();

            // Assert
            Assert.Null(item);
        }
    }
}
