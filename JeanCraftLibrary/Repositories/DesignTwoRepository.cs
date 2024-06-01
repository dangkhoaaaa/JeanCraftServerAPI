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
    public class DesignTwoRepository : GenericRepository<DesignTwo> , IDesignTwoRepository
    {
        private readonly JeanCraftContext _dbContext;

        public DesignTwoRepository(JeanCraftContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DesignTwo> CreateDesignTwo(DesignTwo designTwo)
        {
            var createDesignTwo = new DesignTwo
            {
                DesignTwoId = Guid.NewGuid(),
                Finishing = designTwo.Finishing,
                FabricColor = designTwo.FabricColor
            };

            _dbContext.DesignTwos.Add(designTwo);
            await _dbContext.SaveChangesAsync();

            return designTwo;
        }

        public async Task<List<DesignTwo>> GetAllDesignTwos()
        {
            return await _dbContext.DesignTwos.ToListAsync();
        }

        public async Task<DesignTwo> GetDesignTwoById(Guid designTwoId)
        {
            return await _dbContext.DesignTwos.FindAsync(designTwoId);
        }
    }
}
