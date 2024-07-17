using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IComponentTypeRepository : IGenericRepository<ComponentType>
    {
        Task<IEnumerable<ComponentType>> GetAllComponent(string? search, int currentPage, int pageSize);

        Task<IEnumerable<ComponentType>> GetComponentById(Guid ComponentTypeId);
        Task<ComponentType> CreateComponent(ComponentType ComponentType);
        Task<ComponentType> UpdateComponent(ComponentType componentType);
        Task<bool> DeleteComponent(Guid ComponentTypeId);
    }
}
