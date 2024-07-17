using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories
{
    public class ComponentTypeRepository : GenericRepository<ComponentType>, IComponentTypeRepository
    {
        private readonly JeanCraftContext _context;

        public ComponentTypeRepository(JeanCraftContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<ComponentType> CreateComponent(ComponentType componentType)
        {
            if (await _context.ComponentTypes.AnyAsync(c => c.TypeId == componentType.TypeId))
            {
                throw new Exception("TypeId already exists in the database.");
            }
            _context.ComponentTypes.Add(componentType);
            await _context.SaveChangesAsync();
            return componentType;
        }

        public async Task<bool> DeleteComponent(Guid ComponentTypeId)
        {
            var ComponentType = await _context.ComponentTypes.FindAsync(ComponentTypeId);
            if(ComponentType == null)
            {
                return false;
            }
            _context.ComponentTypes.Remove(ComponentType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ComponentType>> GetAllComponent(string? search, int currentPage, int pageSize)
        {
            var query = _context.Set<ComponentType>().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.Description.Contains(search));
            }

            return await query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<ComponentType>> GetComponentById(Guid ComponentTypeId)
        {
            return await _context.ComponentTypes.Where(c => c.TypeId == ComponentTypeId).ToArrayAsync();
        }

        public async Task<ComponentType> UpdateComponent(ComponentType componentType)
        {
            var existingEntity = await _context.ComponentTypes.FirstOrDefaultAsync(c => c.TypeId == componentType.TypeId);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(componentType);
            }
            else
            {
                _context.ComponentTypes.Update(componentType);
            }
            await _context.SaveChangesAsync();
            return componentType;
        }


    }
}
