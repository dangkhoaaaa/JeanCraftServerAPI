using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Repositories.Interface;
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

        public Task<DesignThree> CreateDesignThree(DesignThree designThree)
        {
            throw new NotImplementedException();
        }

        public Task<List<DesignThree>> GetAllDesignThrees()
        {
            throw new NotImplementedException();
        }

        public Task<DesignThree> GetDesignThreeById(Guid designThreeId)
        {
            throw new NotImplementedException();
        }
    }
}
