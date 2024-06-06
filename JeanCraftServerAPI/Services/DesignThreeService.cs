using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
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

        public async Task<DesignThree> CreateDesignThree(DesignThreeRequest designThree)
        {
            return await _unitOfWork.DesignThreeRepository.CreateDesignThree(designThree);
        }

        public async Task<Guid?> FindDesignThreeByComponentsAsync(Guid? stitchingThreadColor, Guid? buttonAndRivet)
        {
            return await _unitOfWork.DesignThreeRepository.FindDesignThreeByComponentsAsync(stitchingThreadColor, buttonAndRivet);
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
