using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories
{
    public class DesignThreeRepository : GenericRepository<DesignThree> , IDesignThreeRepository
    {
        private readonly JeanCraftContext _dbContext;

        public DesignThreeRepository(JeanCraftContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DesignThree> CreateDesignThree(DesignThreeRequest designThree)
        {
            if (designThree.ButtonAndRivet.HasValue && !await _dbContext.Components.AnyAsync(c => c.ComponentId == designThree.ButtonAndRivet.Value))
            {
                throw new ArgumentException("Invalid Button And Rivet ID");
            }

            if (designThree.StitchingThreadColor.HasValue && !await _dbContext.Components.AnyAsync(c => c.ComponentId == designThree.StitchingThreadColor.Value))
            {
                throw new ArgumentException("Invalid Stitching Thread Color Color ID");
            }

            //if (designThree.BranchBackPatch.HasValue && !await _dbContext.Components.AnyAsync(c => c.ComponentId == designThree.StitchingThreadColor.Value))
            //{
            //    throw new ArgumentException("Invalid Stitching Thread Color Color ID");
            //}
            var newdesignThree = new DesignThree
            {
                DesignThreeId = Guid.NewGuid(),
                ButtonAndRivet = designThree.ButtonAndRivet,
                StitchingThreadColor = designThree.StitchingThreadColor,
                //BranchBackPatch = designThree.BranchBackPatch,
            };

            _dbContext.DesignThrees.Add(newdesignThree);
            await _dbContext.SaveChangesAsync();

            var buttonAndRivetNavigation = await _dbContext.Components.FindAsync(newdesignThree.ButtonAndRivet);
            var stitchingThreadColorNavigation = await _dbContext.Components.FindAsync(newdesignThree.StitchingThreadColor);
            //var branchBackPatchNavigation = await _dbContext.Components.FindAsync(newdesignThree.BranchBackPatch);
            return new DesignThree
            {
                DesignThreeId = newdesignThree.DesignThreeId,
                ButtonAndRivet = newdesignThree.ButtonAndRivet,
                StitchingThreadColor = newdesignThree.StitchingThreadColor,
                //BranchBackPatch = newdesignThree.BranchBackPatch,
                ButtonAndRivetNavigation = buttonAndRivetNavigation != null ? new Component
                {
                    ComponentId = buttonAndRivetNavigation.ComponentId,
                    Description = buttonAndRivetNavigation.Description,
                    Image = buttonAndRivetNavigation.Image,
                    Prize = buttonAndRivetNavigation.Prize,
                    Type = buttonAndRivetNavigation.Type
                } : null,
                StitchingThreadColorNavigation = stitchingThreadColorNavigation != null ? new Component
                {
                    ComponentId = stitchingThreadColorNavigation.ComponentId,
                    Description = stitchingThreadColorNavigation.Description,
                    Image = stitchingThreadColorNavigation.Image,
                    Prize = stitchingThreadColorNavigation.Prize,
                    Type = stitchingThreadColorNavigation.Type
                } : null,
                //BranchBackPatchNavigation = branchBackPatchNavigation != null ? new Component
                //{
                //    ComponentId = branchBackPatchNavigation.ComponentId,
                //    Description = branchBackPatchNavigation.Description,
                //    Image = branchBackPatchNavigation.Image,
                //    Prize = branchBackPatchNavigation.Prize,
                //    Type = branchBackPatchNavigation.Type
                //} : null
            };
        }

        public async Task<List<DesignThree>> GetAllDesignThrees()
        {
            return await _dbContext.DesignThrees.Include(d => d.StitchingThreadColorNavigation).ThenInclude(c => c.TypeNavigation)
               .Include(d => d.ButtonAndRivetNavigation).ThenInclude(c => c.TypeNavigation).ToListAsync();
        }

        public async Task<DesignThree> GetDesignThreeById(Guid designThreeId)
        {
            return await _dbContext.DesignThrees.Include(d => d.StitchingThreadColorNavigation).ThenInclude(c => c.TypeNavigation)
               .Include(d => d.ButtonAndRivetNavigation).ThenInclude(c => c.TypeNavigation).FirstOrDefaultAsync(d => d.DesignThreeId == designThreeId);
        }

        public async Task<Guid?> FindDesignThreeByComponentsAsync(Guid? stitchingThreadColor, Guid? buttonAndRivet)
        {
            var designThree = await _dbContext.DesignThrees
                .Where(d => d.StitchingThreadColor == stitchingThreadColor &&
                            d.ButtonAndRivet == buttonAndRivet)
                .FirstOrDefaultAsync();

            return designThree?.DesignThreeId;
        }
    }
}
