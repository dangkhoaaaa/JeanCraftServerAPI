using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IDesignThreeRepository : IGenericRepository<DesignThree>
    {
        Task<DesignThree> CreateDesignThree(DesignThreeRequest designThree);
        Task<List<DesignThree>> GetAllDesignThrees();
        Task<DesignThree> GetDesignThreeById(Guid designThreeId);
        Task<Guid?> FindDesignThreeByComponentsAsync(Guid? stitchingThreadColor, Guid? buttonAndRivet);
    }
}
