using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories
{
    public class ComponentRepsitory : GenericRepository<Component>, IComponentRepsitory
    {
        private readonly JeanCraftContext _dbContext;

        public ComponentRepsitory(JeanCraftContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Component> CreateComponent(ComponentDTO component)
        {
            var componentToAdd = new Component
            {
                ComponentId = Guid.NewGuid(),
                Description = component.Description,
                Image = component.Image,
                Prize = component.Prize,
                Type = component.Type
            };
            _dbContext.Components.Add(componentToAdd);
            await _dbContext.SaveChangesAsync();
            return componentToAdd;

        }

        public async Task<bool> DeleteComponent(Guid componentId)
        {
            var component = await _dbContext.Components.FindAsync(componentId);
            if (component == null) 
            {
                return false;
            }
            _dbContext.Components.Remove(component);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Component>> GetAllComponent()
        {
            return await _dbContext.Components.Include(x => x.TypeNavigation).ToListAsync();
        }

        public async Task<Component> GetComponentById(Guid Id)
        {
            return await _dbContext.Components.Include(x => x.TypeNavigation).AsNoTracking().FirstOrDefaultAsync(c => c.ComponentId == Id);
        }

        public async Task<Component> UpdateComponent(Component component)
        {
            var existingEntity = await _dbContext.Components.FirstOrDefaultAsync(c => c.ComponentId == component.ComponentId);
            if (existingEntity != null)
            {
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(component);
            }
            else
            {
                _dbContext.Components.Update(component);
            }
            await _dbContext.SaveChangesAsync();
            return component;
        }
    }
}
