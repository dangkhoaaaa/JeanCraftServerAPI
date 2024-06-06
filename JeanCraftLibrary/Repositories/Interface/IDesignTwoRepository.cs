using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IDesignTwoRepository : IGenericRepository<DesignTwo>
    {
        Task<List<DesignTwo>> GetAllDesignTwos();
        Task<DesignTwo> GetDesignTwoById(Guid designTwoId);
        Task<DesignTwo> CreateDesignTwo(DesignTwoRequest designTwo);
        Task<Guid?> FindDesignTwoByComponentsAsync(Guid? finishing, Guid? fabricColor);
    }
}
