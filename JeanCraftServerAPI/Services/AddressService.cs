using AutoMapper;
using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Repositories.Interface;
using JeanCraftServerAPI.Services.Interface;

namespace JeanCraftServerAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Address>> GetAddressesByUserId(Guid userId)
        {
            return await _unitOfWork.AddressRepository.GetAddressesByUserId(userId);
        }

        public async Task<Address> GetAddressByIdAndUserId(Guid id, Guid userId)
        {
            var address = await _unitOfWork.AddressRepository.GetAddressById(id);
            if (address != null && address.UserId == userId)
            {
                return address;
            }
            return null;
        }

        public async Task<Address> CreateAddress(Address addressdto)
        {
            if (addressdto.UserId == null || addressdto.UserId == Guid.Empty)
            {
                throw new ArgumentException("UserId is required and cannot be empty.");
            }

            var newAddress = new Address
            {
                Id = Guid.NewGuid(),
                UserId = addressdto.UserId,
                Type = addressdto.Type,
                Detail = addressdto.Detail
            };

            return await _unitOfWork.AddressRepository.CreateAddress(newAddress);
        }

        public async Task<AddressDTO> UpdateAddress(Guid id, AddressDTO addressdto)
        {
            var address = await _unitOfWork.AddressRepository.GetAddressById(id);
            if (address == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(addressdto.Type))
            {
                address.Type = addressdto.Type;
            }
            if (!string.IsNullOrEmpty(addressdto.Detail))
            {
                address.Detail = addressdto.Detail;
            }


            await _unitOfWork.AddressRepository.UpdateAddress(address);

            // Trả về đối tượng AddressDTO đã được cập nhật
            return addressdto;
        }
    

        public async Task<bool> DeleteAddress(Guid id, Guid userId)
        {
            var address = await GetAddressByIdAndUserId(id, userId);
            if (address == null)
            {
                return false;
            }
            return await _unitOfWork.AddressRepository.DeleteAddress(id);
        }
    }
}
