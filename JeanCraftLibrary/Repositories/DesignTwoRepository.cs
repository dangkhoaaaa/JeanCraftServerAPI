using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Model.Response;
using JeanCraftLibrary.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories
{
    public class DesignTwoRepository : GenericRepository<DesignTwo> , IDesignTwoRepository
    {
        private readonly JeanCraftContext _dbContext;

        public DesignTwoRepository(JeanCraftContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DesignTwo> CreateDesignTwo(DesignTwoRequest designTwo)
        {
            if (designTwo.Finishing.HasValue && !await _dbContext.Components.AnyAsync(c => c.ComponentId == designTwo.Finishing.Value))
            {
                throw new ArgumentException("Invalid Finishing ID");
            }

            if (designTwo.FabricColor.HasValue && !await _dbContext.Components.AnyAsync(c => c.ComponentId == designTwo.FabricColor.Value))
            {
                throw new ArgumentException("Invalid Fabric Color ID");
            }
            var newdesignTwo = new DesignTwo
            {
                DesignTwoId = Guid.NewGuid(),
                Finishing = designTwo.Finishing,
                FabricColor = designTwo.FabricColor,
            };

            _dbContext.DesignTwos.Add(newdesignTwo);
            await _dbContext.SaveChangesAsync();

            var finishingNavigation = await _dbContext.Components.FindAsync(newdesignTwo.Finishing);
            var fabricColorNavigation = await _dbContext.Components.FindAsync(newdesignTwo.FabricColor);
            return new DesignTwo
            {
                DesignTwoId = newdesignTwo.DesignTwoId,
                Finishing = newdesignTwo.Finishing,
                FabricColor = newdesignTwo.FabricColor,
                FinishingNavigation = finishingNavigation != null ? new Component
                {
                    ComponentId = finishingNavigation.ComponentId,
                    Description = finishingNavigation.Description,
                    Image = finishingNavigation.Image,
                    Prize = finishingNavigation.Prize,
                    Type = finishingNavigation.Type
                } : null,
                FabricColorNavigation = fabricColorNavigation != null ? new Component
                {
                    ComponentId = fabricColorNavigation.ComponentId,
                    Description = fabricColorNavigation.Description,
                    Image = fabricColorNavigation.Image,
                    Prize = fabricColorNavigation.Prize,
                    Type = fabricColorNavigation.Type
                } : null,
            };
        }

        public async Task<Guid?> FindDesignTwoByComponentsAsync(Guid? finishing, Guid? fabricColor)
        {
            var designTwo = await _dbContext.DesignTwos
                .Where(d => d.Finishing == finishing &&
                            d.FabricColor == fabricColor)
                .FirstOrDefaultAsync();

            return designTwo?.DesignTwoId;
        }

        public async Task<List<DesignTwo>> GetAllDesignTwos()
        {
            return await _dbContext.DesignTwos.Include(d => d.FabricColorNavigation).ThenInclude(c => c.TypeNavigation)
                .Include(d => d.FinishingNavigation).ThenInclude(c => c.TypeNavigation).ToListAsync();
        }

        public async Task<DesignTwo> GetDesignTwoById(Guid designTwoId)
        {
            return await _dbContext.DesignTwos.Include(d => d.FabricColorNavigation).ThenInclude(c => c.TypeNavigation)
                .Include(d => d.FinishingNavigation).ThenInclude(c => c.TypeNavigation).FirstOrDefaultAsync(d => d.DesignTwoId == designTwoId);
        }
    }
}
