using JeanCraftLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IDesignTwoRepository : IGenericRepository<DesignTwo>
    {
        Task<DesignTwo> CreateDesignTwo(DesignTwo designTwo);
        Task<List<DesignTwo>> GetAllDesignTwos();
        Task<DesignTwo> GetDesignTwoById(Guid designTwoId);
    }
}
