using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Repositories;
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
            Product product1 = await _unitOfWork.ProductRepository.CreateProduct(product);
            if (product1 != null) {
                ProductInventory productInventory = await _unitOfWork.ProductInventoryRepository.CreateProductInventory(product.ProductInventory);
            }
            return product1;
        }

        public async Task<Product?> CreateProductByBooking(Product product)
        {
            return await _unitOfWork.ProductRepository.CreateProduct(product);
        }

        public async Task<Product?> DeleteProduct(Product product)
        {
            if(await _unitOfWork.ProductInventoryRepository.DeleteProductInventory(product.ProductId)) { 
                Product product1 = await _unitOfWork.ProductRepository.DeleteProduct(product);
                return await _unitOfWork.ProductRepository.DeleteProduct(product1);
            }
            return null;
        }

        public async Task<Product?> GetProductByID(Guid id)
        {
            return await _unitOfWork.ProductRepository.GetProductByID(id);
        }

        public async Task<Product[]?> GetProductByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> GetProductID(GetProductIDByDesignRequest request)
        {
            return await _unitOfWork.ProductRepository.GetProductID(request);
        }

        public async Task<Product[]?> GetProductList()
        {
            return await _unitOfWork.ProductRepository.GetProductList();
        }

        public async Task<Product[]?> SearchProduct(SearchProductRequest filter)
        {
            filter.MinPrice = filter.MinPrice == null ? 0 : filter.MinPrice;
            filter.MaxPrice = filter.MaxPrice == null ? 0 : filter.MaxPrice;
            filter.MinSize = filter.MinSize == null ? 0 : filter.MinSize;
            filter.MaxSize = filter.MaxSize == null ? 0 : filter.MaxSize;
            return await _unitOfWork.ProductRepository.SearchProduct(filter);
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
            return await _unitOfWork.ProductRepository.UpdateProduct(product);
        }

        public async Task<Product?> UpdateProductByBooking(Product product)
        {
            return await _unitOfWork.ProductRepository.UpdateProduct(product);
        }
    }
}
