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
    }
}
