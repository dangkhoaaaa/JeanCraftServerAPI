using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories
{
    public class ComponentTypeRepository : IComponentTypeRepository
    {
        public Task<ComponentType> CreateComponent(ComponentType ComponentType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteComponent(Guid ComponentTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<ComponentType> GetAllComponent()
        {
            throw new NotImplementedException();
        }

        public Task<ComponentType> GetComponentById(Guid ComponentTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<ComponentType> UpdateComponent(ComponentType ComponentType)
        {
            throw new NotImplementedException();
        }
    }
}
