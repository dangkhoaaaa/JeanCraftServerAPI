using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftServerAPI.Services.Interface;

namespace JeanCraftServerAPI.Services
{
    public class ComponentTypeService : IComponentTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ComponentTypeService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }   
        public async Task<ComponentType> CreateComponent(ComponentType ComponentType)
        {
            return await _unitOfWork.ComponentTypeRepository.CreateComponent(ComponentType);
        }

        public async Task<bool> DeleteComponent(Guid ComponentTypeId)
        {
            var componentType = await GetComponentById(ComponentTypeId);
            if(componentType == null)
            {
                return false;
            }
            return await _unitOfWork.ComponentTypeRepository.DeleteComponent(ComponentTypeId);
        }

        public async Task<IEnumerable<ComponentType>> GetAllComponent(string? search, int currentPage, int pageSize)
        {
            return await _unitOfWork.ComponentTypeRepository.GetAllComponent( search, currentPage, pageSize);
        }

        public async Task<IEnumerable<ComponentType>> GetComponentById(Guid ComponentTypeId)
        {
            return await _unitOfWork.ComponentTypeRepository.GetComponentById(ComponentTypeId);
        }

        public async Task<ComponentType> UpdateComponent(ComponentType ComponentType)
        {
            return await _unitOfWork.ComponentTypeRepository.UpdateComponent(ComponentType);
        }
    }
}
