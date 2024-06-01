using AutoMapper;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Repositories;
using JeanCraftLibrary.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public JeanCraftContext _dbContext { get; }
        public IMapper _mapper { get;}
        public IAddressRepository AddressRepository => new AddressRepository(_dbContext);

        public IProductRepository ProductRepository => new ProductRepository(_dbContext);

        public IUserRepository UserRepository => new UserRepository(_dbContext, _mapper);

        public IComponentTypeRepository ComponentTypeRepository => new ComponentTypeRepository(_dbContext);
        public IShoppingCartRepository ShoppingCartRepository => new ShoppingCartRepository(_dbContext);
        public IComponentRepsitory ComponentRepsitory => new ComponentRepsitory(_dbContext);
        public IDesignOneRepository DesignOneRepository => new DesignOneRepository(_dbContext);
        public IDesignTwoRepository DesignTwoRepository => new DesignTwoRepository(_dbContext);
        public IDesignThreeRepository DesignThreeRepository => new DesignThreeRepository(_dbContext);

        public IOrderDetailRepository OrderDetailRepository => new OrderDetailRepository(_dbContext);
        public IOrderRepository OrderRepository => new OrderRepository(_dbContext);

        public ICartItemRepository CartItemRepository => new CartItemRepository(_dbContext);

        public UnitOfWork(JeanCraftContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            try
            {
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Rollback()
        {
            Console.WriteLine("Transaction rollback");
        }

        public async Task RollbackAsync()
        {
            Console.WriteLine("Transaction rollback");
            await Task.CompletedTask;
        }
    }
}
