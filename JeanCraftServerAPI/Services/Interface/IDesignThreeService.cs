using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IDesignThreeService
    {
        Task<DesignThree> CreateDesignThree(DesignThreeRequest designThree);
        Task<List<DesignThree>> GetAllDesignThrees();
        Task<DesignThree> GetDesignThreeById(Guid designThreeId);
        Task<Guid?> FindDesignThreeByComponentsAsync(Guid? stitchingThreadColor, Guid? buttonAndRivet);
    }
}
