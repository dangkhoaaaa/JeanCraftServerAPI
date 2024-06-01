using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IDesignThreeRepository : IGenericRepository<DesignThree>
    {
        Task<DesignThree> CreateDesignThree(DesignThree designThree);
        Task<List<DesignThree>> GetAllDesignThrees();
        Task<DesignThree> GetDesignThreeById(Guid designThreeId);
    }
}
