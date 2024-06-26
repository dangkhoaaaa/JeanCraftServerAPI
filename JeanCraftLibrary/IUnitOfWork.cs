﻿using JeanCraftLibrary.Repositories.Interface;
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
        public ICartItemRepository CartItemRepository { get; }
        public IComponentTypeRepository ComponentTypeRepository { get; }
        public IComponentRepsitory ComponentRepsitory { get; }

        public IProductRepository ProductRepository { get; }
        public IProductInventoryRepository ProductInventoryRepository { get; }

        public IUserRepository UserRepository { get; }

        public IDesignOneRepository DesignOneRepository { get; }
        public IDesignTwoRepository DesignTwoRepository { get; }
        public IDesignThreeRepository DesignThreeRepository { get; }
        public IOrderRepository OrderRepository { get; }

        public IOrderDetailRepository OrderDetailRepository { get; }
        public IShoppingCartRepository ShoppingCartRepository { get; }

        public IPaymentRepository PaymentRepository { get; }

        
        void Commit();
        Task CommitAsync();
        void Rollback();
        Task RollbackAsync();
    }
}
