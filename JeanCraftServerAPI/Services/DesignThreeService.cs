using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftServerAPI.Services.Interface;

namespace JeanCraftServerAPI.Services
{
    public class DesignThreeService : IDesignThreeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DesignThreeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DesignThree> CreateDesignThree(DesignThree designThree)
        {
            return await _unitOfWork.DesignThreeRepository.CreateDesignThree(designThree);
        }

        public async Task<List<DesignThree>> GetAllDesignThrees()
        {
            return await _unitOfWork.DesignThreeRepository.GetAllDesignThrees();
        }

        public async Task<DesignThree> GetDesignThreeById(Guid designThreeId)
        {
            return await _unitOfWork.DesignThreeRepository.GetDesignThreeById(designThreeId);
        }
    }
}
