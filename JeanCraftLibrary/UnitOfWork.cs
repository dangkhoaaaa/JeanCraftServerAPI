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

        public IAddressRepository AddressRepository => new AddressRepository(_dbContext);

        public IProductRepository ProductRepository => new ProductRepository(_dbContext);

        public IUserRepository UserRepository => new UserRepository(_dbContext);

        

        public IComponentTypeRepository ComponentTypeRepository => new ComponentTypeRepository(_dbContext);

        public UnitOfWork(JeanCraftContext dbContext)
        {
            _dbContext = dbContext;
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
