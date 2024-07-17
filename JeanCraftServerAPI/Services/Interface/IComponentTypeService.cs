using JeanCraftLibrary.Entity;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IComponentTypeService
    {
        Task<IEnumerable<ComponentType>> GetAllComponent(string? search, int currentPage, int pageSize);

        Task<IEnumerable<ComponentType>> GetComponentById(Guid ComponentTypeId);
        Task<ComponentType> CreateComponent(ComponentType ComponentType);
        Task<ComponentType> UpdateComponent(ComponentType ComponentType);
        Task<bool> DeleteComponent(Guid ComponentTypeId);
    }
}
