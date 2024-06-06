using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Model.Response;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IDesignOneService
    {
        Task<List<DesignOne>> GetAllDesignOnes();
        Task<DesignOne> GetDesignOneById(Guid designOneId);
        Task<DesignOneResponse> CreateAsync(DesignOneRequest designOneRequest);
        Task<Guid?> FindDesignOneByComponentsAsync(Guid? fit, Guid? length, Guid? cuffs, Guid? fly, Guid? frontPocket, Guid? backPocket);
    }
}
