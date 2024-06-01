using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Model.Response;
using JeanCraftServerAPI.Services.Interface;

namespace JeanCraftServerAPI.Services
{
    public class DesignOneService : IDesignOneService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DesignOneService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DesignOne>> GetAllDesignOnes()
        {
            return await _unitOfWork.DesignOneRepository.GetAllDesignOnes();
        }

        public async Task<DesignOne> GetDesignOneById(Guid designOneId)
        {
            return await _unitOfWork.DesignOneRepository.GetDesignOneById(designOneId);
        }
        public async Task<DesignOneResponse> CreateAsync(DesignOneRequest designOne)
        {
            return await _unitOfWork.DesignOneRepository.CreateAsync(designOne);
        }
    }
}
