using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Entity;
using JeanCraftServerAPI.Util;
using JeanCraftLibrary.Model.Response;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IPaymentService
    {
        Task<IList<PaymentModel>> GetAll();
        Task<PaymentModel> GetOne(Guid paymentId);

        Task Update(PaymentUpdateRequestModel payment);
        Task Add(PaymentCreateRequestModel payment);
        Task Delete(Guid payment);
        Payment GetDetailOne(Guid id, int currentPage, int pageSize);
        PaymentResult GetAllPaging(int currentPage, int pageSize);
        //SuccessfulPaymentResult GetAllSuccessfulPaymentsWithPaging(int currentPage, int pageSize);
        double GetTotalAmountOfSuccessfulPayments();
        double GetTotalAmountOfPayments();
        int GetTotalCount();
        Task<AmountAndCountForDay> getAAmountAndCountForDay(String date);
    }
}
