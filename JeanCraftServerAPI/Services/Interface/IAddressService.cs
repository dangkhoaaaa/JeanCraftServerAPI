using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAddressesByUserId(Guid userId);
        Task<Address> GetAddressByIdAndUserId(Guid id, Guid userId);
        Task<Address> CreateAddress(Address address);
        Task<AddressDTO> UpdateAddress(Guid id, AddressDTO address);
        Task<bool> DeleteAddress(Guid id, Guid userId);
    }
}
