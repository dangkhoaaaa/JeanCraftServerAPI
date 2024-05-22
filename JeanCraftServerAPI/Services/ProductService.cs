﻿using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Repositories.Interface;
using JeanCraftServerAPI.Services.Interface;

namespace JeanCraftServerAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Product?> CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> GetProductByID(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product[]?> GetProductByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Product>> GetProductList()
        {
            return await _unitOfWork.ProductRepository.GetAllAsync();
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
