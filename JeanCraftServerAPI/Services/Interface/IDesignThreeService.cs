using JeanCraftLibrary.Entity;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IDesignThreeService
    {
        Task<DesignThree> CreateDesignThree(DesignThree designThree);
        Task<List<DesignThree>> GetAllDesignThrees();
        Task<DesignThree> GetDesignThreeById(Guid designThreeId);
    }
}
