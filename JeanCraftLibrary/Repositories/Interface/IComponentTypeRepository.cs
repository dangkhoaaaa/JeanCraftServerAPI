using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IComponentTypeRepository
    {
        Task<ComponentType> GetAllComponent();

        Task<ComponentType> GetComponentById(Guid ComponentTypeId);
        Task<ComponentType> CreateComponent(ComponentType ComponentType);
        Task<ComponentType> UpdateComponent(ComponentType ComponentType);
        Task<bool> DeleteComponent(Guid ComponentTypeId);
    }
}
