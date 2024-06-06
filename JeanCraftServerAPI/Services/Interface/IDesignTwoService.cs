using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IDesignTwoService
    {
        Task<DesignTwo> CreateDesignTwo(DesignTwoRequest designTwo);
        Task<List<DesignTwo>> GetAllDesignTwos();
        Task<DesignTwo> GetDesignTwoById(Guid designTwoId);
        Task<Guid?> FindDesignTwoByComponentsAsync(Guid? finishing, Guid? fabricColor);
    }
}
