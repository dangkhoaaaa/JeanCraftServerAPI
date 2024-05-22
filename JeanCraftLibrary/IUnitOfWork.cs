using JeanCraftLibrary.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary
{
    public interface IUnitOfWork
    {
        public IAddressRepository AddressRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IUserRepository UserRepository { get; }
        void Commit();
        Task CommitAsync();
        void Rollback();
        Task RollbackAsync();
    }
}
