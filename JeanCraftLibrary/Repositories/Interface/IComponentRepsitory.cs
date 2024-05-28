using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IComponentRepsitory : IGenericRepository<Component>
    {
        Task<List<IGrouping<Guid?, Component>>> GetAllComponent();

        Task<Component> GetComponentById(Guid componentId);
        Task<Component> CreateComponent(ComponentDTO component);
        Task<Component> UpdateComponent(Component component);
        Task<bool> DeleteComponent(Guid componentId);
    }
}
