using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Model;

namespace JeanCraftServerAPI.Services.Interface
{
 
    public interface IPaymentService
    {
        Task<IList<PaymentModel>> GetAll();
        Task<PaymentModel> GetOne(Guid paymentId);

        Task Update(PaymentUpdateRequestModel payment);
        Task Add(PaymentCreateRequestModel payment);
        Task Delete(Guid payment);

        PaymentModel GetDetailOne(Guid id, int currentPage, int pageSize);
        IList<PaymentModel> GetAllPaging(int currentPage, int pageSize);
    }
}
