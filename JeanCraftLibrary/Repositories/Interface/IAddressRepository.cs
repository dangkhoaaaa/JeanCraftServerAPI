using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<IEnumerable<Address>> GetAddressesByUserId(Guid userId);
        Task<Address> GetAddressById(Guid addressId);
        Task<Address> CreateAddress(Address address);
        Task<Address> UpdateAddress(Address address);
        Task<bool> DeleteAddress(Guid addressId);
    }
}
