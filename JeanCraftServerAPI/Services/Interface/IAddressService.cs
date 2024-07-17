using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAddressesByUserId(Guid userId);
        Task<Address> GetAddressByIdAndUserId(Guid id, Guid userId);
        Task<Address> CreateAddress(AddressRequest address);
        Task<AddressDTO> UpdateAddress(Guid id, AddressDTO address);
        Task<bool> DeleteAddress(Guid id, Guid userId);
    }
}
