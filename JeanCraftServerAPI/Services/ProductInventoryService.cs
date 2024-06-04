using AutoMapper;
using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;
using JeanCraftServerAPI.Services.Interface;

namespace JeanCraftServerAPI.Services
{
    public class ProductInventoryService : IProductInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductInventoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductInventory> CreateProductInventory(ProductInventoryRequest productInventoryRequest)
        {
            var productInventory = _mapper.Map<ProductInventory>(productInventoryRequest);
            return await _unitOfWork.ProductInventoryRepository.CreateProductInventory(productInventory);
        }

        public async Task<bool> DeleteProductInventory(Guid id)
        {
            var deleteProdcutInventory = await _unitOfWork.ProductInventoryRepository.GetProductInventoryByID(id);
            if (deleteProdcutInventory != null)
            {
                return await _unitOfWork.ProductInventoryRepository.DeleteProductInventory(id);
            }
            return false;
        }

        public async Task<ProductInventory> GetProductInventoryByID(Guid id)
        {
            return await _unitOfWork.ProductInventoryRepository.GetProductInventoryByID(id);
        }

        public async Task<IEnumerable<ProductInventory>> GetProductInventorys()
        {
            return await _unitOfWork.ProductInventoryRepository.GetProductInventorys();
        }

        public async Task<ProductInventory> UpdateProductInventory(ProductInventoryRequest productInventoryRequest)
        {
            var productInventory = _mapper.Map<ProductInventory>(productInventoryRequest);
            return await _unitOfWork.ProductInventoryRepository.UpdateProductInventory(productInventory);
        }
    }
}
