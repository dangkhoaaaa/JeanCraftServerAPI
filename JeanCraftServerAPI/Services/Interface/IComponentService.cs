using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IComponentService
    {
        Task<List<IGrouping<Guid?, Component>>> GetAllComponent();

        Task<Component> GetComponentById(Guid componentId);
        Task<Component> CreateComponent(ComponentDTO component);
        Task<Component> UpdateComponent(Guid id, ComponentDTO component);
        Task<bool> DeleteComponent(Guid componentId);
    }
}
