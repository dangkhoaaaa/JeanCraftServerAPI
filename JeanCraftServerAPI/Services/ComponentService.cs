using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftServerAPI.Services.Interface;

namespace JeanCraftServerAPI.Services
{
    public class ComponentService : IComponentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ComponentService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Component> CreateComponent(ComponentDTO component)
        {
            return await _unitOfWork.ComponentRepsitory.CreateComponent(component);
        }

        public async Task<bool> DeleteComponent(Guid componentId)
        {
            var deleteComponent = await _unitOfWork.ComponentRepsitory.GetComponentById(componentId);
            if(deleteComponent == null)
            {
                return false;
            }
            return await _unitOfWork.ComponentRepsitory.DeleteComponent(componentId);
        }

        public async Task<List<IGrouping<Guid?, Component>>> GetAllComponent(string? search, int currentPage, int pageSize)
        {
            return await _unitOfWork.ComponentRepsitory.GetAllComponent(search, currentPage, pageSize);
        }

        public async Task<Component> GetComponentById(Guid componentId)
        {
            return await _unitOfWork.ComponentRepsitory.GetComponentById(componentId);
        }

        public async Task<Component> UpdateComponent(Guid id, ComponentDTO componentDto)
        {
            var component = await _unitOfWork.ComponentRepsitory.GetComponentById(id);
            if (component == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(componentDto.Description))
            {
                component.Description = componentDto.Description;
            }
            if (!string.IsNullOrEmpty(componentDto.Image))
            {
                component.Image = componentDto.Image;
            }
            if (componentDto.Prize.HasValue)
            {
                component.Prize = componentDto.Prize.Value;
            }
            if (componentDto.Type.HasValue)
            {
                component.Type = componentDto.Type.Value;
            }

            await _unitOfWork.ComponentRepsitory.UpdateComponent(component);
            return component;

        }
    }
}
