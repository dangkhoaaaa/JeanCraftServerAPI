using JeanCraftLibrary.Entity;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IDesignTwoService
    {
        Task<DesignTwo> CreateDesignTwo(DesignTwo designTwo);
        Task<List<DesignTwo>> GetAllDesignTwos();
        Task<DesignTwo> GetDesignTwoById(Guid designTwoId);
    }
}
