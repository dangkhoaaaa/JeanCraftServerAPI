using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftServerAPI.Services.Interface;

namespace JeanCraftServerAPI.Services
{
    public class DesignTwoService : IDesignTwoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DesignTwoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DesignTwo> CreateDesignTwo(DesignTwoRequest designTwo)
        {
            return await _unitOfWork.DesignTwoRepository.CreateDesignTwo(designTwo);
        }

        public async Task<List<DesignTwo>> GetAllDesignTwos()
        {
            return await _unitOfWork.DesignTwoRepository.GetAllDesignTwos();
        }

        public async Task<DesignTwo> GetDesignTwoById(Guid designTwoId)
        {
            return await _unitOfWork.DesignTwoRepository.GetDesignTwoById(designTwoId);
        }
        public async Task<Guid?> FindDesignTwoByComponentsAsync(Guid? finishing, Guid? fabricColor)
        {
            return await _unitOfWork.DesignTwoRepository.FindDesignTwoByComponentsAsync(finishing, fabricColor);
        }
    }
}
